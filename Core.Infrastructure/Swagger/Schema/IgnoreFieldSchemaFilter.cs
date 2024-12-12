using Core.DataAnnotations;
using Core.ExtentionMethods.String;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Core.Infrastructure.Swagger.Schema;

public class CustomAttrbiuteSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        //var customAttribute = context.MemberInfo?.GetCustomAttribute<ConditionalJsonIgnoreAttribute>();

        //if (customAttribute != null)
        //{
        //    if (customAttribute.ShouldIgnore())
        //        model.ReadOnly = true;
        //}

        HandleRquired(model, context);
    }
    private static void HandleRquired(OpenApiSchema model, SchemaFilterContext context)
    {
        var customAttribute = context.MemberInfo?.GetCustomAttribute<CustomRequiredAttribute>();
        if (customAttribute != null)
        {
            if (!customAttribute.ValidationFilter(null))
                model.Required.Add(context.MemberInfo.Name.ToCamelCase());
        }
    }
}

public class CustomAttrbiuteDocumentFilter : IDocumentFilter
{
    public static Assembly ModelAssembly { get; set; }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        List<(string parentName, string name)> ignoreClass = new();

        //if (ModelAssembly != null)
        //{
        //    var conditionalJsonClass = ModelAssembly.GetTypes()
        //  .Where(x => x.GetProperties().Any(p => p.PropertyType.IsClass && p.PropertyType.IsPublic &&
        //  p.CustomAttributes.Any(a => a.AttributeType == typeof(ConditionalJsonIgnoreAttribute))))
        //         .ToList();

        //    foreach (var item in conditionalJsonClass)
        //    {
        //        var properties = item.GetProperties();

        //        foreach (var prop in properties)
        //        {
        //            var attr = prop.GetCustomAttribute<ConditionalJsonIgnoreAttribute?>();
        //            if (attr?.ShouldIgnore() == true)
        //            {
        //                ignoreClass.Add((item.Name, prop.Name.ToCamelCase()));
        //            }
        //        }
        //    }
        //}


        foreach (var schema in context.SchemaRepository.Schemas)
        {
            var backupP = schema.Value.Properties.ToDictionary(x => x.Key, x => x.Value);
            schema.Value.Properties.Clear();


            foreach (var property in backupP)
            {
                HandlerRequired(schema, property);

                if (property.Value.ReadOnly != true && !ignoreClass.Any(x => x.parentName == schema.Key && x.name == property.Key))
                {
                    schema.Value.Properties.Add(property);
                }

            }
        }
    }

    private static void HandlerRequired(KeyValuePair<string, OpenApiSchema> schema, KeyValuePair<string, OpenApiSchema> property)
    {
        var req = property.Value.Required.FirstOrDefault();
        if (req == null)
        {
            schema.Value.Required.Remove(property.Key);
        }
        else
        {
            property.Value.Required.Remove(req);
        }
    }
}
