using System.Data.Common;
using BlazorApp1.Data.Vendor;
using BlazorApp1.Models;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject1;

public abstract class BaseTest
{
    protected const string TableName = "Employee";
    protected ServiceProvider _serviceProvider;
    [SetUp]
    public virtual void Setup()
    {
        
    }

    protected void CallSql(BaseDatabaseVendor vendor, string sql)
    {
        var db = vendor.GetConnection();
        db.Open();
        DbCommand createCommand = db.CreateCommand();
        createCommand.CommandText = sql;
        createCommand.ExecuteNonQuery();
    }

    [TearDown]
    public void Cleanup()
    {
        _serviceProvider.Dispose();
    }

    [Test]
    public virtual void TestGetOne()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<IDatabaseVendor>();
        var result = db.GetOne(TableName, [("Id", 1)]);
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
    public virtual void TestGetAll()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<IDatabaseVendor>();
        var result = db.GetAll(TableName);
        foreach (var entity in result)
        {
            Assert.That(entity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(entity.Ids, Is.Not.Empty);
                Assert.That(entity.Values, Is.Not.Empty);
                Assert.That(entity.TableName, Is.Not.Empty);
            });    
        }
        
    }
    
    [Test]
    public virtual void TestCreateOne()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<IDatabaseVendor>();
        GenericEntity newEntity = db.GetEmpty("Employee");
        newEntity.Values["Avatar"] = "AVATAR"; 
        newEntity.Values["Department"] = "DEPARTMENT"; 
        newEntity.Values["Email"] = "EMAIL"; 
        newEntity.Values["FirstName"] = "FIRSTNAME"; 
        newEntity.Values["LastName"] = "LASTNAME"; 

        db.CreateOne(newEntity);
        newEntity.Ids.Values.ToList().ForEach(x=>Assert.That(x, Is.Not.Null));
        Console.WriteLine("Created:" + newEntity);

        Assert.Multiple(() =>
        {
            Assert.That(newEntity.Ids, Is.Not.Empty);
            Assert.That(newEntity.Values, Is.Not.Empty);
            Assert.That(newEntity.TableName, Is.Not.Empty);
        });
    }
    
    [Test]
    public virtual void TestUpdateOne()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<IDatabaseVendor>();
        var entity = db.GetOne(TableName, [("Id", 1)]);
        entity.Values["Avatar"] = "www";
        entity.Values["Department"] = "Police";
        db.UpdateOne(entity);
        var entity2 = db.GetOne(TableName, [("Id", 1)]);
        Console.WriteLine(entity2);
        Assert.That(entity.Equals(entity2));
    }
    
    [Test]
    public virtual void TestDeleteOne()
    {
        using var scope = _serviceProvider.CreateScope();

        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<IDatabaseVendor>();
        var entity = db.GetOne(TableName, [("Id", 1)]);
        db.DeleteOne(entity);
        GenericEntity? entity2 = null;
        try
        {
            entity2 = db.GetOne(TableName, [("Id", 1)]);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
        }

        Assert.That(entity2 is null);
    }

    protected string GetResource(string name)
    {
        var runningDir = TestContext.CurrentContext.TestDirectory;
        var filePath = Path.Join(runningDir, name.Replace("/", Path.DirectorySeparatorChar.ToString()));
        return File.ReadAllText(filePath);
    }
    
}