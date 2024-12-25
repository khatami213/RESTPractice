using Core.Contract.Infrastructure;
using System.Linq.Expressions;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Models.ReadModels.Users;

namespace Authorization.Domain.Repositories.Users;

public interface IUserRepositor : IRepository { }

public interface IUserWriteRepository : IWriteRepository<User>
{
    Task DeleteById(long id);
}

public interface IUserReadRepository : IReadRepository<UserReadModel>
{
    Task<UserReadModel?> GetByUsername(string username, params Expression<Func<UserReadModel, object>>[] includes);
    Task<UserReadModel?> GetWithRolesPermissions(string username);
}
