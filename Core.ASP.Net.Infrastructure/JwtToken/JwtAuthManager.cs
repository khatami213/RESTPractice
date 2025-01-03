﻿using Core.ASP.Net.Infrastructure.JwtToken.Abstractions;
using Core.ASP.Net.Infrastructure.JwtToken.Models;
using Core.Contract.Application.Jwt;
using Core.Contract.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core.ASP.Net.Infrastructure.JwtToken;

public class JwtAuthManager : IJwtAuthManager
{
    private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;  // can store in a database or a distributed cache
    public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
    private readonly JwtTokenConfig _jwtTokenConfig;
    private readonly byte[] _secret;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JwtAuthManager(JwtTokenConfig jwtTokenConfig, IServiceScopeFactory serviceScopeFactory)
    {
        _jwtTokenConfig = jwtTokenConfig;
        _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
        _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
        _serviceScopeFactory = serviceScopeFactory;
    }

    // optional: clean up expired refresh tokens
    public void RemoveExpiredRefreshTokens(DateTime now)
    {
        var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpiryDate < now).ToList();
        foreach (var expiredToken in expiredTokens)
        {
            _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
        }

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var refreshTokenService = scope.ServiceProvider.GetRequiredService<IJwtRefreshTokenService>();
            refreshTokenService.CleanupExpiredTokens();
        }
    }

    // can be more specific to ip, user agent, device name, etc.
    public void RemoveRefreshTokenByUserName(string userName)
    {
        var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserName == userName).ToList();
        foreach (var refreshToken in refreshTokens)
        {
            _usersRefreshTokens.TryRemove(refreshToken.Key, out _);
        }

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var refreshTokenService = scope.ServiceProvider.GetRequiredService<IJwtRefreshTokenService>();
            refreshTokenService.RemoveRefreshTokensByUsername(userName);
        }
    }

    public JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
    {
        var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
        var jwtToken = new JwtSecurityToken(
            _jwtTokenConfig.Issuer, 
            shouldAddAudienceClaim ? _jwtTokenConfig.Audiences.First() : string.Empty, // Use first audience as default //shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
            claims.Concat(_jwtTokenConfig.Audiences.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud))), //claims,
            expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        Int32.TryParse(claims.Where(x => x.Type == "UserId").FirstOrDefault().Value, out int userId);

        var refreshToken = new RefreshToken
        {
            UserId = userId,
            UserName = username,
            Token = GenerateRefreshTokenString(),
            ExpiryDate = now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
        };
        _usersRefreshTokens.AddOrUpdate(refreshToken.Token, refreshToken, (_, _) => refreshToken);

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var refreshTokenService = scope.ServiceProvider.GetRequiredService<IJwtRefreshTokenService>();
            refreshTokenService.GenerateRefreshToken(refreshToken);
        }

        return new JwtAuthResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
    {
        var (principal, jwtToken) = DecodeJwtToken(accessToken);
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
        {
            throw new SecurityTokenException("Invalid token");
        }

        var userName = principal.Identity?.Name;
        if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
        {
            throw new SecurityTokenException("Invalid token");
        }
        if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpiryDate < now)
        {
            throw new SecurityTokenException("Invalid token");
        }

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var refreshTokenService = scope.ServiceProvider.GetRequiredService<IJwtRefreshTokenService>();
            if (!refreshTokenService.ValidateRefreshToken(refreshToken, userName))
                throw new SecurityTokenException("Invalid Token");

            refreshTokenService.RevokeRefreshToken(refreshToken);
        }

        return GenerateTokens(userName, principal.Claims.ToArray(), now); // need to recover the original claims
    }

    public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new SecurityTokenException("Invalid token");
        }
        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidateAudience = true,
                    ValidAudiences = _jwtTokenConfig.Audiences,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                },
                out var validatedToken);
        return (principal, validatedToken as JwtSecurityToken);
    }

    private static string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
            ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }
}
