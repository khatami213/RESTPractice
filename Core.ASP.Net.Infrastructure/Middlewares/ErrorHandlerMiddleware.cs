using Core.ASP.Net.Infrastructure.ExtentionMethods;
using Core.Contract.Application.Exceptions;
using Core.Contract.Errors;
using Core.Contract.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Core.ASP.Net.Infrastructure.Middlewares;

public static class ErrorHandlerMiddleware
{
    public static void UseErrorHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (BusinessException ex)
            {
                var responseBody = ex.ErrorCode + " - " + ex.ErrorMessage;

                logger.LogError("Method Input : 400 - ResponseBody: {0}", responseBody);

                await context.ResponseBusinessError(new Result(false, new List<Error>() { new(ex.ErrorCode ?? " null ", ex.ErrorMessage) }));
            }
            catch (Exception ex)
            {
                var queryString = context.Request.QueryString.Value;

                logger.LogError(ex, "Method Input : 500 - {QueryString} , RequestBody: {RequestBody}", queryString, "");

                if (ex.InnerException != null && ex.InnerException.GetType().Name.ToLower().Contains("sql"))
                {
                    throw ex.InnerException;
                }

                await context.ResponseInternalServerError();
            }
        });
    }
}
