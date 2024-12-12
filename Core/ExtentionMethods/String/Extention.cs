namespace Core.ExtentionMethods.String;

public static class Extention
{
    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        // حذف فاصله ها
        string withoutSpaces = str.Replace(" ", string.Empty);

        // تبدیل حرف اول به lowercase
        return char.ToLower(withoutSpaces[0]) + withoutSpaces.Substring(1);
    }

    public static string ToScreamingSnakeCase(this string input)
    {
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
    }

    public static bool ExistLastValueAfterKey(this string @string, string key, string value, bool ignoreCase = true)
    {
        if (@string != null && @string.LastIndexOf(key) > 0)
        {
            var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            var result = @string.Remove(0, @string.LastIndexOf(key) + key.Length).ToLower();

            return string.Equals(result, value, stringComparison);
        }

        return false;
    }

}
