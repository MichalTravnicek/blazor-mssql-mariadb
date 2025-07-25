namespace BlazorApp1.Models;

public class ConnectionContext
{
    public string DbProvider { get; set; } = "";
    public string Sqlite { get; set; } = "";
    public string SqliteOut { get; set; } = "";
    public string MariaDb { get; set; } = "";
    public string MsSqlDb { get; set; } = "";
}