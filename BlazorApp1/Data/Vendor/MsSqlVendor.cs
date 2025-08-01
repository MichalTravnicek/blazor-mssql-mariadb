using System.Data.SqlTypes;
using BlazorApp1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace BlazorApp1.Data.Vendor;

public class MsSqlVendor(IOptions<ConnectionContext> connectionContext)
    : BaseDatabaseVendor(() => new SqlConnection(connectionContext.Value.MsSqlDb))
{
    public override string GetVendorName() => "MsSqlServer";

    public override void CreateOne(GenericEntity entity)
    {
        var ids = String.Join(", ", entity.Ids.Keys.Select(x => "INSERTED." + x));
        CreateOne(entity,$"INSERT INTO @TABLE@ (@COLUMNS@) OUTPUT {ids} VALUES (@VALUES@)");
    }
    public override List<string> GetAllTables()
    {
        string query = "SELECT * FROM information_schema.tables;";
        List<GenericEntity> entries = SelectQuery(query);
        List<string> enumerable = entries.Select(entity => entity.Values["TABLE_NAME"].ToString()).ToList();
        return enumerable;
    }

    public override GenericEntity GetEmpty(string tableName)
    {
        var blankEntity = new GenericEntity();
        blankEntity.TableName = tableName;
        var select = "SELECT col.name AS COLUMN_NAME," +
                     " TYPE_NAME(col.system_type_id) AS DATA_TYPE, " +
                     "CASE WHEN (COALESCE(ic.column_id, idc.column_id)) IS NOT NULL THEN 'true' ELSE 'false' END AS IS_KEY " +
                     "FROM sys.tables tab  " +
                     "   INNER JOIN sys.indexes pk" +
                     "   ON pk.object_id = tab.object_id" +
                     "       AND pk.is_primary_key = 1      " +
                     "   INNER JOIN sys.columns col " +
                     "     ON col.object_id = tab.object_id" +
                     "   LEFT JOIN sys.index_columns ic" +
                     "         ON col.column_id = ic.column_id" +
                     "           AND ic.object_id = col.object_id     " +
                     "           AND ic.index_id = pk.index_id " +
                     "    LEFT JOIN sys.identity_columns idc  " +
                     "         ON col.column_id = idc.column_id" +
                     "          AND idc.object_id = col.object_id" +
                     $"  WHERE tab.name = '{tableName}'" +
                     " ORDER BY col.column_id";
        List<GenericEntity> entries = SelectQuery(select);
        foreach (var selectEntity in entries)
        {
            var column = selectEntity.Values["COLUMN_NAME"]?.ToString() ?? 
                         throw new SqlTypeException("Column name missing");
            var dataType = ColumnTypeMapping(selectEntity.Values["DATA_TYPE"]?.ToString() ?? string.Empty);
            var isId = Boolean.Parse(selectEntity.Values["IS_KEY"]?.ToString() ?? string.Empty);
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
        switch (type)
        {
            case "int" : return 0;
            case "varchar" : return "";
            case "uniqueidentifier": return Guid.NewGuid(); 
        }

        throw new SqlTypeException("Unknown type encountered");
    }
    
}
