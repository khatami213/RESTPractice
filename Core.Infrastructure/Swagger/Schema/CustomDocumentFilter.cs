using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Infrastructure.Swagger.Schema;

public class CustomDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var orderdTags = swaggerDoc.Tags.OrderBy(x => x.Name).ToList();
        swaggerDoc.Tags = orderdTags;
    }
}
