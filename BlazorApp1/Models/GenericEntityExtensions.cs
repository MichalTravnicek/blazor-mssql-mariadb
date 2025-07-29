using System.Data;

namespace BlazorApp1.Models;

public static class GenericEntityExtensions
{
    public static DataTable ToDataTable(this IEnumerable<GenericEntity> entities)
    {
        Console.WriteLine("To data table conversion");
        DataTable table = new DataTable();

        var genericEntities = entities.ToList();
        foreach (var column in genericEntities.First().ColumnsInfo())
        {
            table.Columns.Add(column.Key, column.Value.Type);
        }
        
        foreach (var item in genericEntities)
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
    
}