using Core.Contract.Config;
using Core.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Core.Infrastructure.Swagger.Schema;

namespace Core.Infrastructure.Config.Startup;

public static class SwaggerExtention
{

    public static IServiceCollection AddSwaggerService(
        this IServiceCollection services, 
        AuthorizeConfigOptions authorizeConfig, 
        string title,
        Assembly modelAssembly = null)
    {

        services.AddSwaggerGen(cfg =>
        {

            cfg.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = "v1" });
            //cfg.SchemaFilter<AssignPropertyRequiredFilter>();
            var xmlCommentsFile = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
            xmlCommentsFile.ForEach(xmlFile => cfg.IncludeXmlComments(xmlFile, true));
            //cfg.SchemaFilter<EnumSchemaFilter>();

            //todo temp
            //CustomAttrbiuteDocumentFilter.ModelAssembly = modelAssembly;
            //cfg.SchemaFilter<CustomAttrbiuteSchemaFilter>();
            //cfg.DocumentFilter<CustomAttrbiuteDocumentFilter>();
            //cfg.DocumentFilter<CustomDocumentFilter>();

            //cfg.DocumentFilter<HideActionsFilter>();

            //if (authorizeConfig.AuthorizeType == "IdentityServer4")
            //{

                cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement {
                  {
                    new OpenApiSecurityScheme
                    {
                      Reference = new OpenApiReference
                      {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                      }
                     },
                     new string[] { }
                   }
                 });

            //}

        });

        return services;
    }

}
public class AssignPropertyRequiredFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var requiredProperties = context.Type.GetProperties()
            .Where(x => x.IsDefined(typeof(CustomRequiredAttribute), false))
            .Select(t => char.ToLowerInvariant(t.Name[0]) + t.Name.Substring(1));

        if (schema.Required == null)
        {
            schema.Required = new HashSet<string>();
        }
        schema.Required = schema.Required.Union(requiredProperties).ToHashSet();
    }
}
