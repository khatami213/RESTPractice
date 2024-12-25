using Front.Domain.Models.Projects;
using Front.Services.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Front.Services.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly JwtSecurityTokenHandler _jwtTokenHandler;
    private readonly AuthenticationState _anonymous;
    private readonly NavigationManager _navigationManager;
    private readonly IContainerStorage _containerStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJSRuntime _jsRuntime;

    public CustomAuthStateProvider(NavigationManager navigationManager,
        IContainerStorage containerStorage,
        IHttpContextAccessor httpContextAccessor,
        IJSRuntime jsRuntime)
    {
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        _jwtTokenHandler = new JwtSecurityTokenHandler();
        _navigationManager = navigationManager;
        _containerStorage = containerStorage;
        _httpContextAccessor = httpContextAccessor;
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //var token = await _localStorage.GetAsync<string>("authToken");
        var token = string.Empty;
            _containerStorage.GetItem<string>("authToken");

        if (_httpContextAccessor.HttpContext != null)
        {
            if (_httpContextAccessor.HttpContext.Request.Method == HttpMethod.Get.Method)
                token = _httpContextAccessor.HttpContext.Request.Cookies["authToken"];
            else
                token = await _jsRuntime.InvokeAsync<string>("ReadCookie", "authToken");
        }
        else
            token = await _jsRuntime.InvokeAsync<string>("ReadCookie", "authToken");

        if (string.IsNullOrEmpty(token))
            return _anonymous;

        var jwtToken = _jwtTokenHandler.ReadJwtToken(token);
        var claims = ParseClaims(jwtToken);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        if (!identity.IsAuthenticated)
        {
            _navigationManager.NavigateTo("/login", true);
        }

        return new AuthenticationState(user);
    }

    public void NotifyUserAuthentication(string token)
    {
        var jwtToken = _jwtTokenHandler.ReadJwtToken(token);
        var claims = ParseClaims(jwtToken);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public Task<List<Project>>? GetProjects()
    {
        var projects = _containerStorage.GetItem<List<Project>>("userProjects");
        return Task.FromResult(projects);
    }

    public async Task Logout()
    {
        _containerStorage.RemoveItem("authToken");
        _containerStorage.RemoveItem("userProjects");

        NotifyUserLogout();

        _navigationManager.NavigateTo("/", true);
    }

    public Task<string?> GetToken()
    {
        var token = _containerStorage.GetItem<string>("authToken");
        return Task.FromResult<string?>(token);
    }

    private IEnumerable<Claim> ParseClaims(JwtSecurityToken token)
    {
        var claims = token.Claims.ToList();
        //claims.Add(new Claim(ClaimTypes.Name, token.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value));
        return claims;
    }
}
