using Authorization.Application.Services;
using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Repositories.Users;

namespace Authorization.Services.Services;

public class RoleService : IRoleService
{
    private readonly IRoleReadRepository _roleReadRepository;
    private readonly IRoleWriteRepository _roleWriteRepository;

    public RoleService(IRoleReadRepository roleReadRepository, IRoleWriteRepository roleWriteRepository)
    {
        _roleReadRepository = roleReadRepository;
        _roleWriteRepository = roleWriteRepository;
    }

    public Task<Role?> Create(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RoleReadModel>?> GetAll()
    {
        return await _roleReadRepository.GetAll();
    }

    public async Task<RoleReadModel?> GetById(long id)
    {
        return await _roleReadRepository.GetById(id, r => r.Permissions);
    }

    public async Task<RoleReadModel?> GetByName(string name)
    {
        return await _roleReadRepository.GetByName(name, r => r.Permissions);
    }
}
