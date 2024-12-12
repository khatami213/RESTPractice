using Core.Contract.Application.Commands;
using Core.Contract.Application.Events;
using Core.Contract.Response;

namespace Core.Contract.Facade;

public interface IFacadeCommandService : IFacadeCommand
{
    Task<Result> CreateProcess<TCommand>(TCommand command) where TCommand : ICommand;
    Task<Result> Process<TCommand>(TCommand command) where TCommand : ICommand;
    Task<Result> Process<TCommand, TEvent>(TCommand command) where TCommand : ICommand where TEvent : Event;
    Task<TResult> ProcessResult<TCommand, TResult>(TCommand command) where TCommand : ICommand;
}
