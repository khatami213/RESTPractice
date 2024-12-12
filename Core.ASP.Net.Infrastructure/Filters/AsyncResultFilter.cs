using Core.ASP.Net.Infrastructure.ExtentionMethods;
using Core.Contract.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Core.ASP.Net.Infrastructure.Filters;

public class AsyncResultFilter : IAsyncResultFilter
{
    public AsyncResultFilter()
    {

    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {

            var model = objectResult?.Value as Result;

            if (model is not null)
            {
                if (model.Success is false)
                    await context.HttpContext.ResponseBusinessError(model);
                else
                    await context.HttpContext.ResponseResult(model);
            }
            else if (objectResult?.StatusCode >= StatusCodes.Status400BadRequest)
            {
                await context.HttpContext.CustomError(new Result(false, null, objectResult?.Value), (HttpStatusCode)objectResult.StatusCode);
            }
            else
            {
                await context.HttpContext.ResponseQueryResult(new Result(true, null, objectResult?.Value));
            }

        }
        else
            await next();
    }
}
