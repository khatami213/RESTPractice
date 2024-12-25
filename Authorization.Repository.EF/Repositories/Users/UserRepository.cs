using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Repositories.Users;
using Core.Contract.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authorization.Repository.EF.Repositories.Users;

public class UserWriteRepository : WriteRepository<User>, IUserWriteRepository
{
    private readonly MainDataDbContext _mainDataDbContext;

    public UserWriteRepository(MainDataDbContext mainDataDbContext) : base(mainDataDbContext)
    {
        _mainDataDbContext = mainDataDbContext;
    }

    public Task DeleteById(long id)
    {
        throw new NotImplementedException();
    }
}

public class UserReadRepository : ReadRepository<UserReadModel>, IUserReadRepository
{
    private readonly MainDataReadDbContext _context;

    public UserReadRepository(MainDataReadDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserReadModel?> GetByUsername(string username, params Expression<Func<UserReadModel, object>>[] includes)
    {
        var query = _context.Users.Where(x => x.Username == username).AsQueryable();
        if(includes.Count() != 0)
            query = includes.Aggregate(query,(current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync();
    }

    public async Task<UserReadModel?> GetWithRolesPermissions(string username)
    {
        return await _context.Users.Include(u => u.Projects).Include(u => u.Roles).ThenInclude(r => r.Permissions).Where(x => x.Username == username).FirstOrDefaultAsync();
    }
}
