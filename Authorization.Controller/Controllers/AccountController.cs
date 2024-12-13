using Authorization.Command.Contract.Commands.Accounts;
using Core.ASP.Net.Infrastructure.JwtToken.Abstractions;
using Core.ASP.Net.Infrastructure.JwtToken.Models;
using Core.Contract.Facade;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Authorization.Controller.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IFacadeCommandService _commandService;
    private readonly IFacadeQueryService _queryService;
    private readonly IJwtAuthManager _jwtAuthManager;

    public AccountController(IFacadeCommandService commandService,
        IFacadeQueryService queryService,
        IJwtAuthManager jwtAuthManager)
    {
        _commandService = commandService;
        _queryService = queryService;
        _jwtAuthManager = jwtAuthManager;
    }

    [AllowAnonymous]
    [HttpPost("login1")]
    public async Task<ActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _commandService.ProcessResult<LoginCommand, LoginResult>(command);
        return Ok(result);
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

    //[HttpPost("logout")]
    //public ActionResult Logout()
    //{
    //    // optionally "revoke" JWT token on the server side --> add the current token to a block-list
    //    // https://github.com/auth0/node-jsonwebtoken/issues/375

    //    var userName = User.Identity?.Name;
    //    _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
    //    return Ok();
    //}

    //[AllowAnonymous]
    //[HttpPost("refresh-token")]
    //public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    //{
    //    try
    //    {
    //        var userName = User.Identity?.Name;
    //        //_logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

    //        if (string.IsNullOrWhiteSpace(request.RefreshToken))
    //            return Unauthorized();

    //        var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
    //        var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
    //        //_logger.LogInformation($"User [{userName}] has refreshed JWT token.");
    //        return Ok(new LoginResult
    //        {
    //            //Username = userName,
    //            //Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
    //            AccessToken = jwtResult.AccessToken,
    //            RefreshToken = jwtResult.RefreshToken
    //        });
    //    }
    //    catch (SecurityTokenException e)
    //    {
    //        return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
    //    }
    //}

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
