using Core.ExtentionMethods.String;
using System.ComponentModel;

namespace Core.ExtentionMethods.Base;

public class EnumTemplate
{
    public int Id { get; set; }

    public string Title { get; set; }
}

public class EnumTemplateForGraphQl
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
}

public static class EnumExtensionMethods
{
    public static List<EnumTemplate> GetItems(this Type enumType)
    {
        if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T is not System.Enum");

        var enumValList = new List<EnumTemplate>();

        foreach (var e in Enum.GetValues(enumType))
        {
            var fi = e.GetType().GetField(e.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            enumValList.Add(new EnumTemplate
            {
                Title = (attributes.Length > 0) ? attributes[0].Description : e.ToString(),
                Id = (int)e
            });
        }

        return enumValList;
    }

    public static List<EnumTemplateForGraphQl> GetItemsForGraphQl(this Type enumType)
    {

        if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T is not System.Enum");

        var enumValList = new List<EnumTemplateForGraphQl>();

        foreach (var e in Enum.GetValues(enumType))
        {
            var fi = e.GetType().GetField(e.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            enumValList.Add(new EnumTemplateForGraphQl
            {
                Title = (attributes.Length > 0) ? attributes[0].Description : e.ToString(),
                Id = e.ToString().ToScreamingSnakeCase()
            });
        }

        return enumValList;
    }
}
