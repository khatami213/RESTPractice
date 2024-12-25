using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;

namespace Authorization.Application.Services;

public interface IRoleService
{
    Task<RoleReadModel?> GetById(long id);
    Task<RoleReadModel?> GetByName(string name);
    Task<Role?> Create(string name);
    Task<IEnumerable<RoleReadModel>?> GetAll();
}
