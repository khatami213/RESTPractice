using Authorization.Query.Contract.Queries.Users;
using Authorization.Command.Contract.Commands.Users;
using Core.Contract.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authorization.Domain.Models.WriteModels.Users;

namespace Authorization.Controller.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IFacadeCommandService _commandService;
    private readonly IFacadeQueryService _queryService;

    public UserController(IFacadeCommandService commandService,
        IFacadeQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUser command)
    {
        var result = await _commandService.CreateProcess(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<User?>> GetUser([FromQuery] GetUserById query)
    {
        var result = await _queryService.Fetch(query);
        return Ok(result);
    }

}
