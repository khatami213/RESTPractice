using Core.Contract.RequestInfos;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Core.ASP.Net.Infrastructure.Filters;

public class ResourceFilter : IResourceFilter
{
    private readonly RequestInfoService _requestInfoService;

    public ResourceFilter(RequestInfoService requestInfoService)
    {
        _requestInfoService = requestInfoService;
    }
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        string? username;
        int userId = 0;
        //int userType = 0;

        username = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name)?.FirstOrDefault()?.Value;

        if (username == null)
        {
            var claimsIdentity = context.HttpContext.Request.HttpContext.User.Identity as ClaimsIdentity;
            username = claimsIdentity.FindFirst("Name")?.Value;
            userId = int.Parse(claimsIdentity.FindFirst("Id")?.Value ?? "0");
            //userType = int.Parse(claimsIdentity.FindFirst("UserType")?.Value ?? "0");
        }

        _requestInfoService.UserName = username;
        _requestInfoService.UserId = userId;
        _requestInfoService.ClientAccessToken = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();
        //_requestInfoService.UserType = ((UserTypeEnum)userType);
    }
}
