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
        return GetEmptyInternal($"SELECT * FROM {tableName} LIMIT 1", tableName);
    }
}
