using Core.Contract.Application.Queries;
using Core.Contract.Facade;

namespace Core.Application.Facade.Queries;

public class FacadeQueryService : FacadeQuery,IFacadeQueryService
{
    private readonly IQueryBus _queryBus;

    public FacadeQueryService(IQueryBus queryBus)
    {
        _queryBus = queryBus;
    }

    public async Task<TResult> Fetch<TResult>(IQuery<TResult> query)
    {
        return await _queryBus.Dispatch(query);
    }
}
