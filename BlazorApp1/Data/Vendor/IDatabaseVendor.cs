using BlazorApp1.Models;

namespace BlazorApp1.Data.Vendor;

public interface IDatabaseVendor
{   /// <summary>
    /// Get name of database vendor
    /// </summary>
    /// <returns></returns>
    string GetVendorName();
    /// <summary>
    /// Get list of all tables in database
    /// </summary>
    /// <returns></returns>
    List<string> GetAllTables();
    /// <summary>
    /// Gets blank entity for saving new
    /// </summary>
    /// <returns></returns>
    GenericEntity GetEmpty(string tableName);
    /// <summary>
    /// Gets all enitites in table
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    List<GenericEntity> GetAll(string tableName);
    /// <summary>
    /// Gets one entity based on supplied ids
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    GenericEntity GetOne(string tableName, (string, object)[] ids);
    /// <summary>
    /// Saves entity and update it with database ids
    /// </summary>
    /// <param name="entity"></param>
    void CreateOne(GenericEntity entity);
    /// <summary>
    /// Updates supplied entity in database
    /// </summary>
    /// <param name="entity"></param>
    void UpdateOne(GenericEntity entity);
    /// <summary>
    /// Deletes supplied entity from database
    /// </summary>
    /// <param name="entity"></param>
    void DeleteOne(GenericEntity entity);

}