using System.Linq.Expressions;

namespace Core.Contract.Infrastructure;

public interface IRepository
{

}

public interface IWriteRepository<T> : IRepository where T : class
{
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}

public interface IReadRepository<T> : IRepository where T : class
{
    Task<T?> GetById(long id, params Expression<Func<T, object>>[] includes);
    Task<IReadOnlyList<T?>> GetAll(params Expression<Func<T, object>>[] includes);
}
