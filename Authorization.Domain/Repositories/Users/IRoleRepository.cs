using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Core.Contract.Infrastructure;
using System.Linq.Expressions;

namespace Authorization.Domain.Repositories.Users;

public interface IRoleRepository
{

}

public interface IRoleWriteRepository : IWriteRepository<Role>
{

}

public interface IRoleReadRepository : IReadRepository<RoleReadModel>
{
    public Task<RoleReadModel?> GetByName(string name, params Expression<Func<RoleReadModel, object>>[] includes);
}
