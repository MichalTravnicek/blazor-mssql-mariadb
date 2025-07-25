using BlazorApp1.Models;

namespace BlazorApp1.Data.Vendor;

public interface IDatabaseVendor
{
    List<string> GetAllTables();
    List<GenericEntity> GetAll(string tableName);
    GenericEntity GetOne(string tableName, (string, object)[] ids);
    void CreateOne(GenericEntity entity);
    void UpdateOne(GenericEntity entity);
    void DeleteOne(GenericEntity entity);

}