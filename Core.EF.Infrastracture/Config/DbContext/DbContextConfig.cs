using Core.ExtentionMethods.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Infrastracture.Config.DbContext;

public static class DbContextConfig
{
    public static Dictionary<string,Type> Configs { get; private set; }

    static DbContextConfig()
    {
        Configs = new Dictionary<string,Type>();
    }

    public static void Add(string key, Type dbContextType)
    {
        Configs.Add(key, dbContextType);
    }

    public static Type GetContext(string @namespace)
    {
        foreach (var config in Configs)
            if (@namespace.ExistLastValueAfterKey(".", config.Key))
                return config.Value;

        return null;
    }

}
