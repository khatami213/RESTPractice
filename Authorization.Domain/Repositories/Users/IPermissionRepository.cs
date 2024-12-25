using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Core.Contract.Infrastructure;

namespace Authorization.Domain.Repositories.Users;

public interface IPermissionRepository
{
}

public interface IPermissionWriteRepository : IWriteRepository<Permission>
{

}

public interface IPermissionReadRepository : IReadRepository<PermissionReadModel>
{

}