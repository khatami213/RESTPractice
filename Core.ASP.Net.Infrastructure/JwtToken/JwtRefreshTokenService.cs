using Core.ASP.Net.Infrastructure.JwtToken.Abstractions;
using Core.ASP.Net.Infrastructure.JwtToken.Models;
using Core.Contract.Application.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;

namespace Core.ASP.Net.Infrastructure.JwtToken;

public class JwtRefreshTokenService : IJwtRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtTokenConfig _jwtTokenConfig;

    public JwtRefreshTokenService(IRefreshTokenRepository refreshTokenRepository, JwtTokenConfig jwtTokenConfig)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenConfig = jwtTokenConfig;
    }

    public void CleanupExpiredTokens()
    {
        var threshold = DateTime.Now.AddDays(-1 * _jwtTokenConfig.RefreshTokenExpiration);

        var tokensToDelete = _refreshTokenRepository.GetExpiredTokens(threshold);

        _refreshTokenRepository.DeleteRange(tokensToDelete.ToList());
    }

    public RefreshTokenModel GenerateRefreshToken(RefreshToken refreshToken)
    {
        var refreshTokenModel = new RefreshTokenModel
        {
            UserId = refreshToken.UserId,
            UserName = refreshToken.UserName,
            Token = refreshToken.Token, //GenerateRefreshTokenString(),
            ExpiryDate = refreshToken.ExpiryDate,//DateTime.Now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration),
            CreatedOn = DateTime.Now,
            CreatedBy = refreshToken.UserName
        };

        _refreshTokenRepository.Create(refreshTokenModel);

        return refreshTokenModel;
    }

    public void RevokeAllUserTokens(long userId)
    {
        var filters = new Expression<Func<RefreshTokenModel, bool>>[]
        {
            x => x.IsRevoked == false
        };

        var refreshTokens = _refreshTokenRepository.GetByUserId(userId, filters);

        foreach (var item in refreshTokens)
        {
            item.IsRevoked = true;
            item.RevokedOn = DateTime.Now;

            _refreshTokenRepository.Update(item);
        }
    }

    public void RevokeRefreshToken(string token, string? replacementToken = null)
    {
        var refreshToken = _refreshTokenRepository.GetByToken(token);

        if (refreshToken == null)
            throw new SecurityTokenException("Token Not Found");

        refreshToken.IsRevoked = true;
        refreshToken.RevokedOn = DateTime.Now;
        refreshToken.ReplaceByToken = replacementToken;

        _refreshTokenRepository.Update(refreshToken);
    }

    public bool ValidateRefreshToken(string token, string username)
    {
        //var refreshTokens = await _refreshTokenRepository.GetByUserId(userId);
        //var refreshToken = refreshTokens.FirstOrDefault(x => x.Token == token);

        var refreshTokens = _refreshTokenRepository.GetByUsername(username, new Expression<Func<RefreshTokenModel, bool>>[] { x => x.Token == token });
        var refreshToken = refreshTokens.FirstOrDefault();

        if (refreshToken == null)
            return false;
        if (refreshToken.IsUsed || refreshToken.IsRevoked || refreshToken.ExpiryDate < DateTime.Now)
            return false;

        return true;
    }

    public void RemoveRefreshTokensByUsername(string username)
    {
        var refreshTokens = _refreshTokenRepository.GetByUsername(username);
        _refreshTokenRepository.DeleteRange(refreshTokens.ToList());
    }


    //private static string GenerateRefreshTokenString()
    //{
    //    var randomNumber = new byte[32];
    //    using var randomNumberGenerator = RandomNumberGenerator.Create();
    //    randomNumberGenerator.GetBytes(randomNumber);
    //    return Convert.ToBase64String(randomNumber);
    //}
}
