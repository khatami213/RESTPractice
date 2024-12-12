namespace Core.Contract.Application.Queries;

public interface IQueryHandler
{

}

public interface IQueryHandler<in TQuery,TResult> : IQueryHandler where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query);
}
