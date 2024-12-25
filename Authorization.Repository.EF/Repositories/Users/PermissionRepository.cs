using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Repositories.Users;

namespace Authorization.Repository.EF.Repositories.Users;

public class PermissionWriteRepository : WriteRepository<Permission>, IPermissionWriteRepository
{
    private readonly MainDataDbContext _context;
    public PermissionWriteRepository(MainDataDbContext context) : base(context)
    {
        _context = context;
    }
}

public class PermissionReadRepository : ReadRepository<PermissionReadModel>, IPermissionReadRepository
{
    private readonly MainDataReadDbContext _context;
    public PermissionReadRepository(MainDataReadDbContext context) : base(context)
    {
    }
}
