using BlazorApp1.Data.Vendor;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TestProject1;

public class MariaDbTest : MsSqlTest
{
    public override void Setup()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.Configure<ConnectionContext>(builder.
            Configuration.GetSection("ConnectionStrings"));

        builder.Services.AddSingleton<IDatabaseVendor, MariaDbVendor>();
        builder.Services.AddSingleton<BaseDatabaseVendor, MariaDbVendor>();

        _serviceProvider = builder.Services.BuildServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetService<IOptions<ConnectionContext>>();
        context.Value.MariaDb = "host=127.0.0.1;port=3306;user id=root;password=example;database=test;";
        var vendor = scopedServices.GetRequiredService<BaseDatabaseVendor>();
        
        CallSql(vendor, "DROP TABLE IF EXISTS Employee");
        CallSql(vendor, GetResource("Data/MariaDb.sql"));
        
        vendor.GetAllTables();
    }
    
}