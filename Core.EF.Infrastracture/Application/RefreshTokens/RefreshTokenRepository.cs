using Core.Contract.Application.Jwt;
using Core.EF.Infrastracture.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.EF.Infrastracture.Application.RefreshTokens;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly CoreDbContext _context;

    public RefreshTokenRepository(CoreDbContext context)
    {
        _context = context;
    }

    public RefreshTokenModel Create(RefreshTokenModel model)
    {
        var entry = _context.RefreshTokenModels.Add(model);
        _context.SaveChanges();
        return entry.Entity;
    }

    public void Delete(RefreshTokenModel model)
    {
        _context.Remove(model);
        _context.SaveChanges();
    }

    public void DeleteRange(List<RefreshTokenModel> refreshTokens)
    {
        _context.RemoveRange(refreshTokens);
        _context.SaveChanges();
    }

    public IEnumerable<RefreshTokenModel> GetExpiredTokens(DateTime threshold)
    {
        return _context.RefreshTokenModels
            .Where(x =>
                x.ExpiryDate < DateTime.Now &&
                (x.IsUsed || x.IsRevoked || x.CreatedOn < threshold))
            .ToList();
    }

    public RefreshTokenModel GetByToken(string token)
    {
        return _context.RefreshTokenModels.SingleOrDefault(x => x.Token == token);
    }

    public IEnumerable<RefreshTokenModel>? GetByUserId(long userId, Expression<Func<RefreshTokenModel,bool>>[] filters = null)
    {
        var query = _context.RefreshTokenModels.Where(x => x.UserId == userId).AsQueryable();

        if (filters.Count() > 0)
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
            
        return query.ToList();
    }

    public IEnumerable<RefreshTokenModel> GetByUsername(string username, Expression<Func<RefreshTokenModel, bool>>[] filters = null)
    {
        var query = _context.RefreshTokenModels.Where(x => x.UserName == username).AsQueryable();

        if (filters.Count() > 0)
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        return query.ToList();
    }

    public RefreshTokenModel Update(RefreshTokenModel model)
    {
        var entry = _context.Update(model);
        _context.SaveChanges();
        return entry.Entity;
    }
}
