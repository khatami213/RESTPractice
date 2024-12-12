using Core.Contract.Application.Queries;

namespace Core.Contract.Factories;

public interface IQueryHandlerFactory
{
    IQueryHandler<TQuery, TResult> Get<TQuery, TResult>() where TQuery : IQuery<TResult>;
    object? Get(Type type);
}
