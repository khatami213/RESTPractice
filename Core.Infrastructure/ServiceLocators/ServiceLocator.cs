using Core.Contract.ServiceLocators;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.ServiceLocators;

public class ServiceLocator : IServiceLocator
{
    private readonly IServiceProvider _provider;

    public ServiceLocator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public T Resolve<T>()
    {
        return _provider.GetService<T>();
    }

    public object Resolve(Type type)
    {
        return _provider.GetService(type);
    }

    public IEnumerable<T> ResolveAll<T>()
    {
        return _provider.GetServices<T>();
    }
}
