namespace Core.Contract.ServiceLocators;

public interface IServiceLocator
{
    T Resolve<T>();
    public object Resolve(Type type);
    IEnumerable<T> ResolveAll<T>();
}
