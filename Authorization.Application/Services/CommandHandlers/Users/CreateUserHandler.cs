using Authorization.Application.Exceptions.Users;
using Authorization.Command.Contract.Commands.Users;
using Authorization.Command.Contract.Events.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Repositories;
using Core.Application.Commands;
using Core.Contract.Application.Commands;
using Core.Contract.RequestInfos;
using Core.Utilities;

namespace Authorization.Application.Services.CommandHandlers.Users;

public class CreateUserHandler : ICommandHandler<CreateUser>
{
    private readonly CommandHandlerService _commandHandlerService;
    private readonly IUserWriteRepository _userWriteRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly RequestInfoService _requestInfoService;

    public CreateUserHandler(CommandHandlerService commandHandlerService, 
        IUserWriteRepository userWriteRepository,
        IUserReadRepository userReadRepository,
        RequestInfoService requestInfoService)
    {
        _commandHandlerService = commandHandlerService;
        _userWriteRepository = userWriteRepository;
        _userReadRepository = userReadRepository;
        _requestInfoService = requestInfoService;
    }

    public async Task Handle(CreateUser command)
    {
        var user = await _userReadRepository.GetByUsername(command.Username);

        if (user != null)
            throw new UserIsDuplicatedException();

        if (command.Password != command.ConfirmPassword)
            throw new PasswordAndConfirmPasswordAreNotEqualException();

        var newUser = _commandHandlerService.Map<CreateUser, User>(command);

        var salt = PasswordUtility.GenerateSalt();
        var hashPassword = PasswordUtility.ComputeHash(command.Password, salt);

        newUser.Password = hashPassword;
        newUser.Salt = salt;
        newUser.CreatedBy = _requestInfoService.UserName;
        newUser.CreatedOn = DateTime.UtcNow;

        await _userWriteRepository.Create(newUser);

        _commandHandlerService.Publish(new UserCreated(newUser.Id));
    }
}
