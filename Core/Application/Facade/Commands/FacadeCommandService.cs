using Core.Contract.Application.Commands;
using Core.Contract.Application.Events;
using Core.Contract.Facade;
using Core.Contract.Response;

namespace Core.Application.Facade.Commands;

public class FacadeCommandService : FacadeCommand, IFacadeCommandService
{
    private readonly ICommandBus _commandBus;

    public FacadeCommandService(ICommandBus commandBus, IEventBus eventBus) : base(eventBus)
    {
        _commandBus = commandBus;
    }
    public async Task<Result> CreateProcess<TCommand>(TCommand command) where TCommand : ICommand
    {
        long? id = null;

        _eventBus.Subscribe<CreatedEvent>(x =>
        {
            id = x.Id;
        });

        await _commandBus.Dispatch(command);

        return Return(id);
    }

    public async Task<Result> Process<TCommand>(TCommand command) where TCommand : ICommand
    {
        await _commandBus.Dispatch(command);

        return Return();
    }

    public async Task<Result> Process<TCommand, TEvent>(TCommand command) where TCommand : ICommand where TEvent : Event
    {
        object? data = null;

        _eventBus.Subscribe<TEvent>(e =>
        {
            data = e;
        });

        await _commandBus.Dispatch(command);

        return Return(data);
    }

    public async Task<TResult> ProcessResult<TCommand, TResult>(TCommand command) where TCommand : ICommand
    {
        return await _commandBus.Dispatch<TCommand, TResult>(command);
    }
}
