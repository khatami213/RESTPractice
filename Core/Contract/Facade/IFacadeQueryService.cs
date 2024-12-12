using Core.Contract.Application.Queries;

namespace Core.Contract.Facade;

public interface IFacadeQueryService : IFacadeQuery
{
    Task<TResult> Fetch<TResult>(IQuery<TResult> query);
}
