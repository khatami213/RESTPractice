using Core.Contract.Application.Commands;
using Core.Contract.Factories;
using Core.Contract.ServiceLocators;

namespace Core.Factories;

public class CommandHandlerFactory : ICommandHandlerFactory
{
    private readonly IServiceLocator _provider;

    public CommandHandlerFactory(IServiceLocator provider)
    {
        _provider = provider;
    }

    public ICommandHandler<T> Get<T>() where T : ICommand
    {
        return _provider.Resolve<ICommandHandler<T>>(); 
    }

    public ICommandHandler<T1, T2> Get<T1, T2>() where T1 : ICommand
    {
        return _provider.Resolve<ICommandHandler<T1, T2>>();
    }

    public IEnumerable<ICommandHandler<T>> GetAll<T>() where T : ICommand
    {
        return _provider.ResolveAll<ICommandHandler<T>>();
    }
}
