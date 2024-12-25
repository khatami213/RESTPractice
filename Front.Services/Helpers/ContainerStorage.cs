using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front.Services.Helpers;

public class ContainerStorage : IContainerStorage
{
    private readonly Dictionary<string, object> _storage = new();

    public void SetItem<TValue>(string key, TValue value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        _storage[key] = value;
    }

    public TValue GetItem<TValue>(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        if (!_storage.ContainsKey(key))
            return default;

        try
        {
            return (TValue)_storage[key];
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException($"Unable to cast stored value to type {typeof(TValue)}");
        }
    }

    public void RemoveItem(string key)
    {
        _storage.Remove(key);
    }
}
