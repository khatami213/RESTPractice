using Core.ASP.Net.Infrastructure.JwtToken.Abstractions;
using Core.ASP.Net.Infrastructure.JwtToken.Models;
using Core.Contract.Application.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core.ASP.Net.Infrastructure.JwtToken;

public static class JwtTokenExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, JwtTokenConfig jwtTokenConfig)
    {
        services.AddSingleton(jwtTokenConfig);
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
        {

            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtTokenConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                ValidAudiences = jwtTokenConfig.Audiences,
                ValidateAudience = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
        });
        IdentityModelEventSource.ShowPII = true;

        services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
        services.AddScoped<IJwtRefreshTokenService, JwtRefreshTokenService>();
        services.AddHostedService<JwtRefreshTokenCache>();

        return services;
    }
}
