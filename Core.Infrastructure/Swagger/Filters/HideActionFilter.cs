//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace Core.Infrastructure.Swagger.Filters;

//public class HideActionsFilter : IDocumentFilter
//{
//    private readonly IPublishConfigService _publishConfigService;

//    public HideActionsFilter(IPublishConfigService publishConfigService) => _publishConfigService = publishConfigService;

//    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
//    {
//        try
//        {
//            var publishConfigs = _publishConfigService.GetPublishConfigsAsync().Result;

//            var includeActions = publishConfigs
//                .Where(c => c.ShouldPublish)
//                .Select(c => new { Path = c.Route.ToLower(), c.Method })
//                .ToList();

//            foreach (var path in swaggerDoc.Paths)
//            {
//                var pathItem = path.Value;

//                var pathValue = path.Key.ToLower();

//                if (includeActions.Where(x => x.Path == pathValue).Any())
//                {
//                    var actions = includeActions.Where(x => x.Path == pathValue).Select(x => (int)x.Method).ToList();
//                    var pathActions = pathItem.Operations.Where(x => !actions.Contains((int)x.Key)).ToList();

//                    pathActions.ForEach(x => pathItem.Operations.Remove(x));
//                }
//                else
//                {
//                    pathItem.Operations.Clear();
//                    swaggerDoc.Paths.Remove(path.Key);
//                }
//            }

//            var controllerList = includeActions.Select(x => x.Path.ToLower()).ToList();

//            var tagsToRemove = swaggerDoc.Tags
//                .Where(tag => !controllerList.Contains(tag.Name.ToLower()))
//                .ToList();


//            tagsToRemove.ForEach(x => swaggerDoc.Tags.Remove(x));


//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error in HideActionsFilter: {ex.Message}");
//        }
//    }
//}
