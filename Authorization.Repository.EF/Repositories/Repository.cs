using Core.Contract.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Authorization.Repository.EF.Repositories;

public class Repository : IRepository
{
}

public class WriteRepository<T> : IWriteRepository<T> where T : class
{
    private readonly MainDataDbContext _context;
    private readonly DbSet<T> _dbSet;

    public WriteRepository(MainDataDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task Create(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public virtual async Task Update(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }
}

public class ReadRepository<T> : IReadRepository<T> where T : class
{
    private readonly MainDataReadDbContext _context;
    private readonly DbSet<T> _dbSet;

    public ReadRepository(MainDataReadDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IReadOnlyList<T?>> GetAll(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes.Count() != 0)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetById(long id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes.Count() != 0)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        // Create a parameter expression
        var parameter = Expression.Parameter(typeof(T), "x");

        // Create property access expression
        var propertyAccess = Expression.Property(parameter, "Id");

        // Create constant for the id value
        var constantId = Expression.Constant(id);

        // Create equality comparison
        var equals = Expression.Equal(propertyAccess, constantId);

        // Create lambda expression
        var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

        return await query.SingleOrDefaultAsync(lambda);

    }
}
