using Core.Contract.Application.Commands;

namespace Core.Contract.Factories;

public interface ICommandHandlerFactory
{
    ICommandHandler<T> Get<T>() where T : ICommand;
    IEnumerable<ICommandHandler<T>> GetAll<T>() where T : ICommand;
    ICommandHandler<T1, T2> Get<T1, T2>() where T1 : ICommand;
}
