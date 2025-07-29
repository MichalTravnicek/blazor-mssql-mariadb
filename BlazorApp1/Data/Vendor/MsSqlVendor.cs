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
        return GetEmptyInternal($"SELECT TOP 1 * FROM {tableName}", tableName);
    }
}
