using BlazorApp1.Data.Vendor;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TestProject1;

public class MsSqlTest : BaseTest
{
    public override void Setup()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.Configure<ConnectionContext>(builder.
            Configuration.GetSection("ConnectionStrings"));

        builder.Services.AddSingleton<IDatabaseVendor, MsSqlVendor>();
        builder.Services.AddSingleton<BaseDatabaseVendor, MsSqlVendor>();

        _serviceProvider = builder.Services.BuildServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetService<IOptions<ConnectionContext>>();
        context.Value.MsSqlDb = "Server=localhost;User Id=sa;Database=Test;Password=Password12345!;Trust Server Certificate=True";
        var vendor = scopedServices.GetRequiredService<BaseDatabaseVendor>();
        
        CallSql(vendor, "IF OBJECT_ID(N'dbo.Employee', N'U') IS NOT NULL DROP TABLE [dbo].[Employee]; ");
        CallSql(vendor, GetResource("Data/MsSql.sql"));
        
        vendor.GetAllTables();
    }
    
    [Test]
    public virtual void TestGetOnePKGUID()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<IDatabaseVendor>();
        var entity = db.GetAll(TableName)[0];
        var result = db.GetOne(TableName, [("PKGUID", entity.Ids["PKGUID"]!)]);
        Console.WriteLine(result);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Ids, Is.Not.Empty);
            Assert.That(result.Values, Is.Not.Empty);
            Assert.That(result.TableName, Is.Not.Empty);
        });
    }
    
    [Test]
    public void TestCreateOnePKGUID()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<IDatabaseVendor>();
        GenericEntity newEntity = new GenericEntity();
        newEntity.TableName = "Employee";
        newEntity.Ids["Id"] = null;
        newEntity.Ids["PKGUID"] = null;
        newEntity.Values["Avatar"] = "AVATAR"; 
        newEntity.Values["Department"] = "DEPARTMENT"; 
        newEntity.Values["Email"] = "EMAIL"; 
        newEntity.Values["FirstName"] = "FIRSTNAME"; 
        newEntity.Values["LastName"] = "LASTNAME"; 
    
        db.CreateOne(newEntity);
        Assert.That(newEntity.Ids["Id"], Is.Not.Null);
        Assert.That(newEntity.Ids["PKGUID"], Is.Not.Null);
        Console.WriteLine(newEntity);
    
        Assert.Multiple(() =>
        {
            Assert.That(newEntity.Ids, Is.Not.Empty);
            Assert.That(newEntity.Values, Is.Not.Empty);
            Assert.That(newEntity.TableName, Is.Not.Empty);
        });
    }
    
}