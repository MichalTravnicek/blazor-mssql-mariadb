using BlazorApp1.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace BlazorApp1.Data.Vendor;

public class SqliteVendor(IOptions<ConnectionContext> connectionContext)
    : BaseDatabaseVendor(() => new SqliteConnection(connectionContext.Value.Sqlite.Replace("/", Path.DirectorySeparatorChar.ToString())))
{
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
    
}
