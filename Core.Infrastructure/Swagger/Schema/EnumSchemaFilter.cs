using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;

namespace Core.Infrastructure.Swagger.Schema;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            model.Enum.Clear();
            foreach (var e in Enum.GetValues(context.Type))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                var title = (attributes.Length > 0) ? attributes[0].Description : e.ToString();


                model.Enum.Add(new OpenApiString(($"{(int)e} = {title}")));

                //$"{(int)e} = {title}"
            }
        }
    }
}
