namespace Front.Services.Helpers;

public interface IContainerStorage
{
    void SetItem<TValue>(string key, TValue value);
    TValue GetItem<TValue>(string key);
    void RemoveItem(string key);
}
