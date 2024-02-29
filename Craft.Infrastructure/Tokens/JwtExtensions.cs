﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Craft.Infrastructure.Tokens;

public static class JwtExtensions
{
    public static void ConfigureJwt(IServiceCollection services, IConfiguration config)
    {
        services
            .AddOptions<JwtSettings>()
            .BindConfiguration($"SecuritySettings:{nameof(JwtSettings)}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = config.GetSection($"SecuritySettings:{nameof(JwtSettings)}").Get<JwtSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                byte[] key = Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);

                options.RequireHttpsMetadata = jwtSettings.RequireHttpsMetaData;
                options.SaveToken = jwtSettings.SaveToken;
                options.IncludeErrorDetails = jwtSettings.IncludeErrorDetails;
                options.Validate(JwtBearerDefaults.AuthenticationScheme);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudiences = jwtSettings.ValidAudiences,
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidateLifetime = jwtSettings.ValidateLifetime,
                    RequireExpirationTime = jwtSettings.RequireExpirationTime,
                    RequireSignedTokens = jwtSettings.RequireSignedTokens,
                    ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkew)
                };
            });
    }
}
