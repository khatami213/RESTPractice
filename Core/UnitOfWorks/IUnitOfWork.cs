namespace Core.UnitOfWorks;

public interface IUnitOfWork
{
    Task BeginTransaction();
    Task Commit();
    Task Rollback();
}
