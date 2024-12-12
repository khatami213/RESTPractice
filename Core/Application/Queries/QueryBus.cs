using Core.Contract.Application.Queries;
using Core.Contract.Application.Validations;
using Core.Contract.Factories;

namespace Core.Application.Queries;

public class QueryBus : IQueryBus
{
    private readonly IValidator _validator;
    private readonly IQueryHandlerFactory _queryHandlerFactory;

    public QueryBus(IValidator validator, IQueryHandlerFactory queryHandlerFactory)
    {
        _validator = validator;
        _queryHandlerFactory = queryHandlerFactory;
    }

    public async Task<TResult> Dispatch<TResult>(IQuery<TResult> query)
    {
        var isValid = _validator.Validate(query);
        if (isValid)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _queryHandlerFactory.Get(handlerType);
            if (handler != null)
                return await (Task<TResult>)handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.Handle)).Invoke(handler, [query]);
            else
                throw new NotImplementedException();
        }
        throw new NotImplementedException();
    }
}
