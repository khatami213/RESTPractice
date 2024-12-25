using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Repository.EF.Repositories.Users;

public class RoleWriteRepository : WriteRepository<Role>, IRoleWriteRepository
{
    private readonly MainDataDbContext _context;
    public RoleWriteRepository(MainDataDbContext context) : base(context)
    {
        _context = context;
    }
}

public class RoleReadRepository : ReadRepository<RoleReadModel>, IRoleReadRepository
{
    private readonly MainDataReadDbContext _context;
    public RoleReadRepository(MainDataReadDbContext context) : base(context)
    {
        context = _context;
    }

    public async Task<RoleReadModel?> GetByName(string name, params Expression<Func<RoleReadModel, object>>[] includes)
    {
        var query = _context.Roles.Where(r => r.Name == name).AsQueryable();
        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.SingleOrDefaultAsync();
    }
}