using Core.Contract.Application.Commands;
using Core.Contract.Application.Validations;
using Core.Contract.Factories;

namespace Core.Application.Commands;

public class CommandBus : ICommandBus
{
    private readonly IValidator _validator;
    //private readonly ICommandHandlerFactory _commandHandlerFactory;
    private readonly ICommandHandlerDecoratorFactory _commandHandlerDecoratorFactory;

    public CommandBus(IValidator validator, ICommandHandlerDecoratorFactory commandHandlerDecoratorFactory)
    {
        _validator = validator;
        _commandHandlerDecoratorFactory = commandHandlerDecoratorFactory;
    }

    public async Task Dispatch<T>(T command) where T : ICommand
    {
        var isValid = _validator.Validate(command);
        if (isValid)
        {
            var handler = _commandHandlerDecoratorFactory.Get<T>();
            if (handler != null)
                await handler.Handle(command);
            else 
                throw new NotImplementedException();
        }
    }

    public async Task<T2> Dispatch<T, T2>(T command) where T : ICommand
    {
        var isValid = _validator.Validate(command);

        if (isValid)
        {
            var handler = _commandHandlerDecoratorFactory.Get<T, T2>();

            if (handler is not null)
            {
                return await handler.Handle(command);
            }
            else
                throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }
}
