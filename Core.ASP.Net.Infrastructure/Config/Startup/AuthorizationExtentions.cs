using Core.ASP.Net.Infrastructure.Authorizations;
using Core.ASP.Net.Infrastructure.JwtToken;
using Core.ASP.Net.Infrastructure.JwtToken.Abstractions;
using Core.ASP.Net.Infrastructure.JwtToken.Models;
using Core.Contract.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core.ASP.Net.Infrastructure.Config.Startup;

public static class AuthorizationExtentions
{
    public static AuthorizeConfigOptions AddAuthorizeService(this WebApplicationBuilder builder)
    {
        var authorizeConfig = builder.Configuration.GetSection("AuthorizeConfig").Get<AuthorizeConfigOptions>();
        builder.Services.AddSingleton(authorizeConfig);

        builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
        builder.Services.AddHostedService<JwtRefreshTokenCache>();

        if (authorizeConfig.AuthorizeType == "BasicAuthentication")
        {
            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
            });

        }
        //else if (authorizeConfig.AuthorizeType == "IdentityServer4")
        //{
        //    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        //        {

        //            options.Authority = authorizeConfig.IdentityServer?.ServerUrl ?? "http://192.168.50.15:44310";

        //            options.RequireHttpsMetadata = false;
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateAudience = false,
        //                ValidateLifetime = true,
        //                ValidateIssuer = false,
        //                ClockSkew = TimeSpan.FromMinutes(3),
        //            };

        //        });
        //}
        else if (authorizeConfig.AuthorizeType == "Jwt")
        {
            var jwtTokenConfig = builder.Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();

            builder.Services.AddServices(jwtTokenConfig);

            //builder.Services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            //{

            //    x.RequireHttpsMetadata = true;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = jwtTokenConfig.Issuer,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
            //        ValidAudience = jwtTokenConfig.Audience,
            //        ValidateAudience = true,
            //        ValidateLifetime = false,
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});
            //IdentityModelEventSource.ShowPII = true;

        }
        else
            throw new Exception($"{authorizeConfig.AuthorizeType} not implemented.");

        return authorizeConfig;
    }
}
