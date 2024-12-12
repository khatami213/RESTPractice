using Core.Contract.Errors;
using Core.Contract.Response;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Core.ASP.Net.Infrastructure.ExtentionMethods;

public static class HttpContextExtensionMethod
{
    public static async Task ResponseInternalServerError(this HttpContext context)
    {
        await Response(context, HttpStatusCode.InternalServerError, new Result(false, new List<Error> { new Error("0", "خطایی در سرور رخ داده است.", null) }));
    }

    public static async Task ResponseResult(this HttpContext context, Result result)
    {
        await Response(context, HttpStatusCode.OK, result);
    }

    public static async Task ResponseQueryResult(this HttpContext context, Result result)
    {
        var httpStatusCode = HttpStatusCode.OK;

        if (result.Data is null)
            httpStatusCode = HttpStatusCode.NotFound;
        //else
        //{
        //    var r = result.Data as PagingResult;

        //    if (r != null)
        //    {
        //        var d  = (typeof(PagingResult<>))r.Result;
        //    }

        //}

        await Response(context, httpStatusCode, result);
    }

    public static async Task ResponseBusinessError(this HttpContext context, Result result)
    {
        await Response(context, HttpStatusCode.BadRequest, result);
    }

    public static async Task CustomError(this HttpContext context, Result result, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        await Response(context, httpStatusCode, result);
    }

    private static async Task Response(HttpContext context, HttpStatusCode httpStatusCode, object data = null)
    {
        context.Response.StatusCode = (int)httpStatusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        //var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore });

        var json = JsonSerializer.Serialize(data, options);

        await context.Response.WriteAsync(json);
    }
}
