using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using BlazorApp1.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace BlazorApp1.Data.Vendor;

public class SqliteVendor(IOptions<ConnectionContext> connectionContext)
    : BaseDatabaseVendor(() => new SqliteConnection(connectionContext.Value.Sqlite.Replace("/", Path.DirectorySeparatorChar.ToString())))
{
    public override string GetVendorName() => "Sqlite";

    public override void CreateOne(GenericEntity entity)
    {
        CreateOne(entity,"INSERT INTO @TABLE@ (@COLUMNS@) VALUES (@VALUES@) RETURNING " + entity.Ids.Keys.First()); 
    }

    public override List<string> GetAllTables()
    {
        string query = "SELECT name FROM sqlite_master WHERE type='table';";
        List<GenericEntity> entries = SelectQuery(query);
        List<string> enumerable = entries.Select(entity => entity.Values["name"].ToString()).ToList();
        return enumerable;
    }

    public override GenericEntity GetEmpty(string tableName)
    {
        var blankEntity = new GenericEntity();
        blankEntity.TableName = tableName;
        var select = $"pragma table_info('{tableName}');";
        List<GenericEntity> entries = SelectQuery(select);
        foreach (var selectEntity in entries)
        {
            var column = selectEntity.Values["name"]?.ToString() ?? 
                         throw new SqlTypeException("Column name missing");
            var dataType = ColumnTypeMapping(selectEntity.Values["type"]?.ToString() ?? string.Empty);
            var isId = selectEntity.Values["pk"]?.ToString() == "1";
            if (isId)
            {
                blankEntity.Ids[column] = dataType;
            }
            else
            {
                blankEntity.Values[column] = dataType;
            }
        }
        return blankEntity;
    }
    
    private object ColumnTypeMapping(string type)
    {
        var _type = Regex.Replace(type,"\\(.*\\)", "");
        switch (_type)
        {
            case "INTEGER" : return 0;
            case "TEXT" : return "";
        }

        throw new SqlTypeException("Unknown type encountered");
    }
}
