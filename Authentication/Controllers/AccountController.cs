using Authorization.Application.Services;
using Core.ASP.Net.Infrastructure.JwtToken.Abstractions;
using Core.ASP.Net.Infrastructure.JwtToken.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Authentication.Controllers;

public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IUserService _userService;
    private readonly IJwtAuthManager _jwtAuthManager;

    public AccountController(ILogger<AccountController> logger, IUserService userService, IJwtAuthManager jwtAuthManager)
    {
        _logger = logger;
        _userService = userService;
        _jwtAuthManager = jwtAuthManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _userService.IsValidUserCredentials(request.Username, request.Password);
        if (user is null)
            return Ok(new { status = false, message = "نام کاربری شما اشتباه می باشد." });

        var roleStr = new StringBuilder();
        var roles = await _userService.GetUserRole(request.Username);
        foreach (var role in roles)
            roleStr.Append($"{role},");

        var claims = new[]
        {
                new Claim(ClaimTypes.Name,request.Username),
                new Claim(ClaimTypes.Role, roleStr.ToString()),
                new Claim("UserId", user.Id.ToString()),
        };

        var jwtResult = _jwtAuthManager.GenerateTokens(request.Username, claims, DateTime.Now);

        _logger.LogInformation($"User [{user.Username}] logged in the system.");

        return Ok(new LoginResult
        {
            Username = user.Username,
            Role = roleStr.ToString(),
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken
        });
    }

    //[HttpGet("user")]
    //[Authorize]
    //public ActionResult GetCurrentUser()
    //{
    //    return Ok(new LoginResult
    //    {
    //        Username = User.Identity?.Name,
    //        Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
    //        OriginalUserName = User.FindFirst("OriginalUserName")?.Value
    //    });
    //}

    [HttpPost("logout")]
    [Authorize]
    public ActionResult Logout()
    {
        // optionally "revoke" JWT token on the server side --> add the current token to a block-list
        // https://github.com/auth0/node-jsonwebtoken/issues/375

        var userName = User.Identity?.Name;
        _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
        _logger.LogInformation($"User [{userName}] logged out the system.");
        return Ok();
    }

    //[AllowAnonymous]
    [HttpPost("refresh-token")]
    [Authorize]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var userName = User.Identity?.Name;
            _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Unauthorized();

            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
            _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
            return Ok(new LoginResult
            {
                //Username = userName,
                //Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken
            });
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
        }
    }

    //[HttpPost("impersonation")]
    //[Authorize(Roles = "admin")]
    //public async Task<ActionResult> Impersonate([FromBody] ImpersonationRequest request)
    //{
    //    var test = request;
    //    var userName = User.Identity?.Name;
    //    throw new NotImplementedException();
    //    User impersonatedUser = null;//await _userService.GetUserByCardGroup(request.CardGroupId);
    //                                 //if (impersonatedUser == null)
    //                                 //    return Ok(new { status = false, message = "نام کاربری یافت نشد" });

    //    _logger.LogInformation($"User [{userName}] is trying to impersonate [{impersonatedUser.Username}].");

    //    //var impersonatedCardGroup = await _userService.GetUserCardGroup(request.Username);
    //    var impersonatedRole = await _userService.GetUserRole(impersonatedUser.Username);
    //    if (string.IsNullOrWhiteSpace(impersonatedRole))
    //    {
    //        _logger.LogInformation($"User [{userName}] failed to impersonate [{impersonatedUser.Username}] due to the target user not found.");
    //        return BadRequest($"The target user [{impersonatedUser.Username}] is not found.");
    //    }
    //    if (impersonatedRole == "admin")
    //    {
    //        _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
    //        return BadRequest("This action is not supported.");
    //    }

    //    var claims = new[]
    //    {
    //        new Claim(ClaimTypes.Name,impersonatedUser.Username),
    //        new Claim(ClaimTypes.Role, impersonatedRole),
    //        new Claim("OriginalUserName", userName ?? string.Empty),
    //        new Claim("CardGroupId", request.CardGroupId.ToString())
    //};

    //    var jwtResult = _jwtAuthManager.GenerateTokens(impersonatedUser.Username, claims, DateTime.Now);
    //    _logger.LogInformation($"User [{impersonatedUser.Username}] is impersonating [{impersonatedUser.Username}] in the system.");
    //    return Ok(new LoginResult
    //    {
    //        Username = impersonatedUser.Username,
    //        Role = impersonatedRole,
    //        CardGroupId = request.CardGroupId.ToString(),
    //        OriginalUserName = userName,
    //        AccessToken = jwtResult.AccessToken,
    //        RefreshToken = jwtResult.RefreshToken.TokenString
    //    });
    //}

    //[HttpPost("stop-impersonation")]
    //public ActionResult StopImpersonation()
    //{
    //    var userName = User.Identity?.Name;
    //    var originalUserName = User.FindFirst("OriginalUserName")?.Value;
    //    if (string.IsNullOrWhiteSpace(originalUserName))
    //    {
    //        return BadRequest("You are not impersonating anyone.");
    //    }
    //    _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");

    //    var role = _userService.GetUserRole(originalUserName);
    //    var claims = new[]
    //    {
    //        new Claim(ClaimTypes.Name,originalUserName),
    //        new Claim(ClaimTypes.Role, role.ToString())
    //    };

    //    var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
    //    _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
    //    return Ok(new LoginResult
    //    {
    //        Username = originalUserName,
    //        Role = role.ToString(),
    //        OriginalUserName = null,
    //        AccessToken = jwtResult.AccessToken,
    //        RefreshToken = jwtResult.RefreshToken.TokenString
    //    });
    //}
}
