using Front.Services.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Front.Services.Handlers.Auth;

public class AuthenticationHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;
    private readonly CustomAuthStateProvider _customAuthStateProvider;

    private readonly List<string> _anonymousEndpoints = new()
    {
        "account/login",
        "account/register",
        "account/forgotpassword"
    };

    public AuthenticationHandler(
        IConfiguration configuration, CustomAuthStateProvider customAuthStateProvider)
    {
        _configuration = configuration;
        _customAuthStateProvider = customAuthStateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var endpoint = request.RequestUri?.PathAndQuery.ToLower();
        if (endpoint != null && _anonymousEndpoints.Any(e => endpoint.Contains(e)))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        try
        {
            var token = await _customAuthStateProvider.GetToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving token: {ex}");
            throw;
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}
