namespace Core.Contract.Application.Queries;

public interface IQueryBus
{
    Task<TResult> Dispatch<TResult>(IQuery<TResult> query);
}
