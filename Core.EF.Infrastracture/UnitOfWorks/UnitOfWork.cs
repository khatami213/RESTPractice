using Core.EF.Infrastracture.EF;
using Core.UnitOfWorks;

namespace Core.EF.Infrastracture.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly CoreDbContext _coreDbContext;

    public UnitOfWork(CoreDbContext coreDbContext)
    {
        _coreDbContext = coreDbContext;
    }

    public async Task BeginTransaction()
    {
        await _coreDbContext.Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        await _coreDbContext.SaveChangesAsync();

        if (_coreDbContext.Database.CurrentTransaction != null)
            await _coreDbContext.Database.CommitTransactionAsync();
    }

    public async Task Rollback()
    {
        if (_coreDbContext.Database.CurrentTransaction != null)
            await _coreDbContext.Database.RollbackTransactionAsync();
    }
}
