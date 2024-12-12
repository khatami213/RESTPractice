using Core.Contract.Application.Queries;
using Core.Contract.Factories;
using Core.Contract.ServiceLocators;
using System;

namespace Core.Factories;

public class QueryHandlerFactory : IQueryHandlerFactory
{
    private readonly IServiceLocator _serviceLocator;

    public QueryHandlerFactory(IServiceLocator serviceLocator)
    {
        _serviceLocator = serviceLocator;
    }

    public IQueryHandler<TQuery, TResult> Get<TQuery, TResult>() where TQuery : IQuery<TResult>
    {
        return _serviceLocator.Resolve<IQueryHandler<TQuery, TResult>>();
    }

    public object? Get(Type type)
    {
        return _serviceLocator.Resolve(type);
    }
}
