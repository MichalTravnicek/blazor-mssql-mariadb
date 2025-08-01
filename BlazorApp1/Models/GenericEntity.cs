namespace BlazorApp1.Models;

public class GenericEntity
{
    public OrderedDictionary<string,object?> Ids { get; set; } = new();
    
    public string? TableName { get; set; }
    public OrderedDictionary<string, object?> Values { get; set; } = new();

    public IEnumerable<KeyValuePair<string, ColumnInfo>> ColumnsInfo()
    {
        List<KeyValuePair<string, ColumnInfo>> columns = [];
        columns.AddRange(Ids.Select(keyValue => 
            new KeyValuePair<string, ColumnInfo>(keyValue.Key, new ColumnInfo(keyValue.Value?.GetType() ?? typeof(int),true))));
        columns.AddRange(Values.Select(keyValue => 
            new KeyValuePair<string, ColumnInfo>(keyValue.Key, new ColumnInfo(keyValue.Value?.GetType() ?? typeof(string),false))));
        return columns;
    }

    public IEnumerable<KeyValuePair<string, object?>> Columns()
    {
        return Ids.Union(Values);
    }

    public class ColumnInfo(Type type, bool isId)
    {
        public Type Type { get; } = type;
        public bool IsId { get; } = isId;
    }
    
    public override string ToString()
    {
        string retval = "";
        foreach (var keyValue in Ids)
        {
            retval += keyValue.Key + "=" + keyValue.Value + ";";
        }
        foreach (var keyValue in Values)
        {
            retval += keyValue.Key + "=" + keyValue.Value + ";";
        }

        return retval;
    }

    protected bool Equals(GenericEntity other)
    {
        return Ids.OrderBy(kvp => kvp.Key)
                   .SequenceEqual(other.Ids.OrderBy(kvp => kvp.Key))
               && TableName == other.TableName &&
               Ids.OrderBy(kvp => kvp.Key)
                   .SequenceEqual(other.Ids.OrderBy(kvp => kvp.Key));
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((GenericEntity)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Ids, TableName, Values);
    }
}