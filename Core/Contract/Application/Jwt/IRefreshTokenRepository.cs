using Core.Contract.Infrastructure;
using System.Linq.Expressions;

namespace Core.Contract.Application.Jwt;

public interface IRefreshTokenRepository : IRepository
{
    RefreshTokenModel Create(RefreshTokenModel model); 
    RefreshTokenModel Update(RefreshTokenModel model);
    void Delete(RefreshTokenModel model);
    void DeleteRange(List<RefreshTokenModel> refreshTokens);
    IEnumerable<RefreshTokenModel>? GetByUserId(long userId, Expression<Func<RefreshTokenModel, bool>>[] filters = null);
    IEnumerable<RefreshTokenModel> GetByUsername(string username, Expression<Func<RefreshTokenModel, bool>>[] filters = null);
    RefreshTokenModel GetByToken(string token);
    IEnumerable<RefreshTokenModel> GetExpiredTokens(DateTime threshold);
}
