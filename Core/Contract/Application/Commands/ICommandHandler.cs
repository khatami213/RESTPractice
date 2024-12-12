namespace Core.Contract.Application.Commands;

public interface ICommandHandler
{
}

public interface ICommandHandler<in T> : ICommandHandler where T : ICommand
{
    Task Handle(T command);
}

public interface ICommandHandler<in T, T2> : ICommandHandler where T : ICommand
{
    Task<T2> Handle(T command);
}
