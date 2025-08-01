using BlazorApp1.Data.Vendor;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TestProject1;

public class SqliteTest : BaseTest
{
    [SetUp]
    public override void Setup()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.Configure<ConnectionContext>(builder.
            Configuration.GetSection("ConnectionStrings"));

        builder.Services.AddSingleton<IDatabaseVendor, SqliteVendor>();
        builder.Services.AddSingleton<BaseDatabaseVendor, SqliteVendor>();

        _serviceProvider = builder.Services.BuildServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetService<IOptions<ConnectionContext>>();
        context.Value.Sqlite = "Data Source=Test.db";
        var vendor = scopedServices.GetRequiredService<BaseDatabaseVendor>();
        
        CallSql(vendor, "DROP TABLE IF EXISTS Employee");
        CallSql(vendor, GetResource("Data/Sqlite.sql"));
        
        vendor.GetAllTables();
    }

    public override void TestEmptyDatabase()
    {
        //not implemented yet
    }

}