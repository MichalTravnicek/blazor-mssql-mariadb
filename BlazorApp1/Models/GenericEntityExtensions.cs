using System.Data;

namespace BlazorApp1.Models;

public static class GenericEntityExtensions
{
    public static DataTable ToDataTable(this IEnumerable<GenericEntity> entities)
    {
        Console.WriteLine("To data table conversion");
        DataTable table = new DataTable();

        foreach (var column in entities.First().ColumnsInfo())
        {
            table.Columns.Add(column.Key, column.Value.Type);
        }
        
        foreach (var item in entities)
        {
            DataRow row = table.NewRow();
            foreach (var keyValue in item.Columns())
            { 
                row[keyValue.Key] = keyValue.Value;
            }
            table.Rows.Add(row);
        }

        return table;
    }

    public static GenericEntity ToEntity(this Dictionary<string,object> data, GenericEntity entity)
    {
        foreach (var keyValue in data)
        {
            if (entity.Ids.Keys.Contains(keyValue.Key))
            {
                entity.Ids[keyValue.Key] = keyValue.Value;
            }
            else
            {
                entity.Values[keyValue.Key] = keyValue.Value;
            }
        }
        return entity;
    }

    
}