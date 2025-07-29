using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Text;
using BlazorApp1.Models;
using static BlazorApp1.Data.Utils;

namespace BlazorApp1.Data.Vendor;

public abstract class BaseDatabaseVendor(Func<DbConnection> dbConnection) : IDatabaseVendor
{
    public readonly Func<DbConnection> GetConnection = dbConnection;

    public abstract string GetVendorName();

    public virtual List<string> GetAllTables()
    {
        throw new NotImplementedException();
    }

    public virtual GenericEntity GetEmpty(string tableName)
    {
        throw new NotImplementedException();
    }
    
    protected GenericEntity GetEmptyInternal(string sql, string tableName)
    {
        var entries = SelectQuery(sql, tableName);
        Console.WriteLine($"Found {tableName} count {entries.Count}");
        var entity = entries.Single();
        entity.Ids.Keys.ToList().ForEach(x => entity.Ids[x] = null);
        entity.Values.Keys.ToList().ForEach(x => entity.Values[x] = null);
        return entity;
    }

    public List<GenericEntity> GetAll(string tableName)
    {
        var query = $"SELECT * FROM {tableName}";
        var entries = SelectQuery(query, tableName);
        Console.WriteLine($"Found {tableName} count {entries.Count}");
        return entries;
    }

    public virtual GenericEntity GetOne(string tableName, (string,object)[] ids)
    {
        var dictionary = ids.ToDictionary(x=>x.Item1, x=>x.Item2);
        var query = $"SELECT * FROM {tableName} WHERE " + CreateIdMatch(dictionary, null);
        var entries = SelectQuery(query, tableName);
        Console.WriteLine($"Found {tableName} count {entries.Count}");
        return entries.Single();
    }

    public virtual void CreateOne(GenericEntity entity)
    {
        CreateOne(entity,$"INSERT INTO @TABLE@ (@COLUMNS@) VALUES (@VALUES@)");
    }

    protected void CreateOne(GenericEntity entity, string query)
    {
        using var db = GetConnection();
        db.Open();
        var createCommand = db.CreateCommand();

        StringBuilder statement = new StringBuilder(query);
        
        statement.Replace("@TABLE@", entity.TableName);
        var columns = entity.Values.Keys.ToList();
        statement.Replace("@COLUMNS@", string.Join(",", columns));
        statement.Replace("@VALUES@", string.Join(",", columns.Select(x => "@" + x)));

        var sql = statement.ToString();
        PrintSql(sql);
        createCommand.CommandText = sql;
        columns.ForEach(x => AddSqlParameter(createCommand, x, entity.Values[x]));
        
        Console.WriteLine("Adding entity:" + entity);
        using var reader = createCommand.ExecuteReader();
        var table = new DataTable();
        table.Load(reader);
        var row = table.Rows[0].ItemArray;
        for (var i = 0; i < row.Length; i++)
        {
            entity.Ids[table.Columns[i].ColumnName] = row[i];
        }
        Console.WriteLine("Added entity with id:" + String.Join(",", 
            entity.Ids.Select(x => x.Key + "=" + x.Value)));
    }

    public void UpdateOne(GenericEntity entity)
    {
        using var db = GetConnection();
        db.Open();
        var updateCommand = db.CreateCommand();

        // Use parameterized query to prevent SQL injection attacks
        var statement = CreateUpdateStatement(entity, updateCommand);
        updateCommand.CommandText = statement;
        Console.WriteLine("Updating entity:" + entity);
        updateCommand.ExecuteNonQuery();
    }

    public void DeleteOne(GenericEntity entity)
    {
        using var db = GetConnection();
        db.Open();
        var deleteCommand = db.CreateCommand();

        var tableName = entity.TableName;
        var statement = $"DELETE FROM {tableName} WHERE (" + CreateIdMatch(entity, deleteCommand) + ")";
        deleteCommand.CommandText = statement;
        PrintSql(statement);
        Console.WriteLine("Deleting entity:" + entity);
        deleteCommand.ExecuteNonQuery();
    }

    protected List<GenericEntity> SelectQuery(string query)
    {
        return SelectQuery(query, "");
    }

    protected List<GenericEntity> SelectQuery(string query, string tableName)
    {
        var entries = new List<GenericEntity>();
        using var db = GetConnection();
        if (db.State != ConnectionState.Open) db.Open();
        var cmd = db.CreateCommand();
        cmd.CommandText = query;
        PrintSql(query);
        
        using var reader = cmd.ExecuteReader();
        var table = new DataTable();
        table.Load(reader);
                
        foreach (DataRow dataRow in table.Rows)
        {
            var entity = new GenericEntity();
            entity.TableName = tableName;
            var primaryKeys = table.PrimaryKey;
            for (var i = 0; i < dataRow.ItemArray.Length; i++)
            {
                var col = table.Columns[i];
                if (col.AutoIncrement || col.Unique || primaryKeys.Contains(col) ||
                    col.DataType == typeof(Guid))
                {
                    entity.Ids.Add(col.ColumnName, dataRow.ItemArray[i]);
                }
                else
                {
                    entity.Values[col.ColumnName] = dataRow.ItemArray[i];
                }
            }
            
            entries.Add(entity);
        }

        PrintResults(table);

        return entries;
    }

    private string CreateUpdateStatement(GenericEntity entity, DbCommand updateCommand)
    {
        var tableName = entity.TableName;
        var sql = new StringBuilder($"UPDATE {tableName} SET @VALUES@ WHERE (@WHERE_CLAUSE@)");
        var values = new StringBuilder();
        foreach (var keyValue in entity.Values)
        {
            AppendValuesString(values, keyValue.Key);
            AddSqlParameter(updateCommand, keyValue.Key, keyValue.Value);
        }

        sql.Replace("@VALUES@", TrimTrailingComma(values.ToString()));
        sql.Replace("@WHERE_CLAUSE@", CreateIdMatch(entity, updateCommand));
        var statement = sql.ToString();
        PrintSql(statement);
        return statement;
    }

    private string CreateIdMatch(GenericEntity entity, DbCommand? command)
    {
        return CreateIdMatch(entity.Ids, command);
    }

    private string CreateIdMatch(IDictionary<string,object?> ids, DbCommand? command)
    {
        var idMatch = new StringBuilder("");
        foreach (var id in ids)
        {
            if (idMatch.Length > 0)
            {
                idMatch.Append(" AND ");
            }
            if (command != null)
            {
                idMatch.Append(id.Key + "=@" + id.Key);    
                AddSqlParameter(command, id.Key, id.Value);
            }
            else
            {
                idMatch.Append(id.Key + "=" + "'" + id.Value + "'");                
            }
            
        }

        if (idMatch is null) throw new SqlTypeException("No id found on entity");
        return idMatch.ToString();
    }
}
