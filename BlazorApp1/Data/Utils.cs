using System.Data;
using System.Data.Common;
using System.Text;

namespace BlazorApp1.Data;

public class Utils
{
    public static void PrintResults(DataTable data)
    {
        Console.WriteLine();
        Dictionary<string, int> colWidths = new Dictionary<string, int>();

        foreach (DataColumn col in data.Columns)
        {
            Console.Write(col.ColumnName.ToUpper());
            var maxLabelSize = data.Rows.OfType<DataRow>()
                .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                .OrderByDescending(m => m).FirstOrDefault();

            colWidths.Add(col.ColumnName, maxLabelSize);
            for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 10; i++) Console.Write(" ");
        }

        Console.WriteLine();

        foreach (DataRow dataRow in data.Rows)
        {
            for (int j = 0; j < dataRow.ItemArray.Length; j++)
            {
                Console.Write(dataRow.ItemArray[j]);
                for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 10; i++) Console.Write(" ");
            }
            Console.WriteLine();
        }
    }

    public static void PrintSql(string sql)
    {
        Console.WriteLine("***********************************************************");
        Console.WriteLine(sql);
        Console.WriteLine("***********************************************************");
    }

    public static void AddSqlParameter(DbCommand command, string name, object value)
    {
        var param = command.CreateParameter();
        param.ParameterName = "@" + name;
        param.Value = value;
        command.Parameters.Add(param);
    }

    public static string TrimTrailingComma(string sql)
    {
        return sql.Substring(0, sql.LastIndexOf(','));
    }

    public static void AppendValuesString(StringBuilder builder, string key)
    {
        builder.Append(key + "=@" + key + ",");
    }
}