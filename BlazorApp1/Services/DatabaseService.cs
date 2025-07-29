using BlazorApp1.Data.Vendor;

namespace BlazorApp1.Services;

public class DatabaseService
{
    private readonly Dictionary<string, IDatabaseVendor> _vendors = new();

    public DatabaseService(IEnumerable<IDatabaseVendor> vendors)
    {
        vendors.ToList().ForEach(x => _vendors.Add(x.GetVendorName(), x));
    }

    public List<string> VendorNames()
    {
        return new List<string>(_vendors.Keys);
    }

    public IDatabaseVendor GetVendor(string name)
    {
        return _vendors[name];
    }
}