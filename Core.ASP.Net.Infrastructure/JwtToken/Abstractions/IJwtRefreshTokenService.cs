using Core.ASP.Net.Infrastructure.JwtToken.Models;
using Core.Contract.Application.Jwt;

namespace Core.ASP.Net.Infrastructure.JwtToken.Abstractions;

public interface IJwtRefreshTokenService
{
    RefreshTokenModel GenerateRefreshToken(RefreshToken refreshToken);
    bool ValidateRefreshToken(string token, string username);
    void RevokeRefreshToken(string token, string? replacementToken = null);
    void RevokeAllUserTokens(long userId);
    void CleanupExpiredTokens();
    void RemoveRefreshTokensByUsername(string username);
}
