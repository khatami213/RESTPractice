using Blazored.LocalStorage;
using Core.Contract.Front;
using Front.Application.Services.Auth;
using Front.Domain.Models.Auth;
using Front.Domain.Models.Projects;
using Front.Services.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace Front.Services.Services.Auth;

public class AuthService : IAuthService
{
    ILogger<AuthService> _logger;
    private readonly HttpClient _httpClient;
    private readonly CustomAuthStateProvider _authenticationStateProvider;
    private readonly IOptions<ApiEndpoint> _apiEndpoint;
    private readonly IContainerStorage _containerStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJSRuntime _jsRuntime;

    public AuthService(IHttpClientFactory httpClientFactory,
        AuthenticationStateProvider authenticationStateProvider,
        IOptions<ApiEndpoint> apiEndpoint,
        ILogger<AuthService> logger,
        IContainerStorage containerStorage,
        IHttpContextAccessor httpContextAccessor,
        IJSRuntime jsRuntime)
    {
        _httpClient = httpClientFactory.CreateClient("AuthClient");
        _authenticationStateProvider = (CustomAuthStateProvider)authenticationStateProvider;
        _apiEndpoint = apiEndpoint;
        _logger = logger;
        _containerStorage = containerStorage;
        _httpContextAccessor = httpContextAccessor;
        _jsRuntime = jsRuntime;
    }

    public async Task<LoginResponse?> Login(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_apiEndpoint.Value.Authentication}Account/Login", request);
        var responseBody = await response.Content.ReadAsStringAsync();
        var model = new { Success = false, Data = new LoginResponse(), Errors = new[] { new { Code = "", Message = "" } } };
        var result = JsonConvert.DeserializeAnonymousType(responseBody, model);

        if (result?.Data.AccessToken != null)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                if (!_httpContextAccessor.HttpContext.Response.HasStarted)
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("authToken", result.Data.AccessToken);
                else
                    await _jsRuntime.InvokeVoidAsync("WriteCookie", "authToken", result.Data.AccessToken, DateTime.Now.AddMinutes(1));
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("WriteCookie", "authToken", result.Data.AccessToken, DateTime.Now.AddMinutes(1));

            }

            _containerStorage.SetItem("authToken", result.Data.AccessToken);

            if (result.Data.Projects.Count > 0)
                _containerStorage.SetItem("userProjects", result.Data.Projects);

            _authenticationStateProvider.NotifyUserAuthentication(result.Data.AccessToken);
        }

        return result.Data;
    }

    public async Task Logout()
    {
        //var token = await _localStorageService.GetItemAsync<string>("authToken");
        //if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _containerStorage.RemoveItem("authToken");
        _containerStorage.RemoveItem("userProjects");
        _authenticationStateProvider.NotifyUserLogout();
    }

    public async Task<UserInfo> GetUserInfo()
    {
        //var token = await _localStorageService.GetItemAsync<string>("authToken");
        ////var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        //if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync($"{_apiEndpoint.Value.Authentication}Account/userinfo");
        var responseBody = await response.Content.ReadAsStringAsync();
        var model = new { Success = false, Data = new UserInfo(), Errors = new[] { new { Code = "", Message = "" } } };
        var result = JsonConvert.DeserializeAnonymousType(responseBody, model);

        return result.Data;
    }
}
