using Core.ASP.Net.Infrastructure.Authorizations;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Core.ASP.Net.Infrastructure.Middlewares;

public class BasicAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    public BasicAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path == "/")
        {
            await _next.Invoke(context);
            return;
        }


        string? authHeader = context.Request.Headers["Authorization"];

        if (authHeader != null && authHeader.StartsWith("Basic"))
        {
            // Get the encoded username and password
            var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

            // Decode from Base64 to string
            var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

            // Split username and password
            var username = decodedUsernamePassword.Split(':', 2)[0];
            var password = decodedUsernamePassword.Split(':', 2)[1];

            //Check if login is correct
            if (IsAuthorized(username, password))
            {
                var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, username);

                context.User.AddIdentity(new ClaimsIdentity(authenticatedUser));

                await _next.Invoke(context);
                return;
            }
        }

        if (context.Request.Path.Value.Contains("api"))
        {
            // Return authentication type (causes browser to show login dialog)
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            // Return unauthorized
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }

    private bool IsAuthorized(string username, string password)
    {
        if (!(username == "TestApi" && password == "1234"))
            return false;

        return true;
    }
}
