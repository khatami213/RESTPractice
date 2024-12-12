namespace Core.ExtentionMethods.Base;

public static class ForEachExtensionMethods
{
    public static void ForEach<T>(this List<T> collection, Action<int, T> action)
    {
        int rowNumber = 1;

        collection.ForEach(item =>
        {
            action(rowNumber, item);
            rowNumber++;
        });
    }
}
