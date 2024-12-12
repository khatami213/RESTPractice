namespace Core.Contract.Application.Commands;

public interface ICommandBus
{
    Task Dispatch<T>(T command) where T : ICommand;
    Task<T2> Dispatch<T, T2>(T command) where T : ICommand;
}
