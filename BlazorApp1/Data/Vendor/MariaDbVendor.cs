using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using BlazorApp1.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace BlazorApp1.Data.Vendor;

public class MariaDbVendor(IOptions<ConnectionContext> connectionContext)
    : BaseDatabaseVendor(() => new MySqlConnection(connectionContext.Value.MariaDb))
{
    public override string GetVendorName() => "MariaDb";

    public override void CreateOne(GenericEntity entity)
    {
        var ids = String.Join(", ", entity.Ids.Keys);
        CreateOne(entity,$"INSERT INTO @TABLE@ (@COLUMNS@) VALUES (@VALUES@) RETURNING {ids}");
    }
    public override List<string> GetAllTables()
    {
        string query = "SHOW TABLE STATUS;";
        List<GenericEntity> entries = SelectQuery(query);
        List<string> enumerable = entries.Select(entity => entity.Values["Name"].ToString()).ToList();
        return enumerable;
    }

    public override GenericEntity GetEmpty(string tableName)
    {
        var blankEntity = new GenericEntity();
        blankEntity.TableName = tableName;
        var select = $"DESCRIBE {tableName};";
        List<GenericEntity> entries = SelectQuery(select);
        foreach (var selectEntity in entries)
        {
            var column = selectEntity.Values["Field"]?.ToString() ?? 
                         throw new SqlTypeException("Column name missing");
            var dataType = ColumnTypeMapping(selectEntity.Values["Type"]?.ToString() ?? string.Empty);
            var isId = selectEntity.Values["Key"]?.ToString()?.Length > 0;
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
            case "int" : return 0;
            case "char" : return "";
            case "varchar" : return "";
        }

        throw new SqlTypeException("Unknown type encountered");
    }
}
