using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Craft.Infrastructure.OpenApi;

public static class OpenApiExtensions
{
    private static readonly string[] value = ["Bearer"];

    private static OpenApiInfo GetOpenApiInfo(SwaggerOptions swaggerSettings)
    {
        return new OpenApiInfo
        {
            Title = swaggerSettings.Title,
            Version = swaggerSettings.Version,
            Description = swaggerSettings.Description,
            Contact = new OpenApiContact
            {
                Name = swaggerSettings.ContactName,
                Email = swaggerSettings.ContactEmail,
                Url = new Uri(swaggerSettings.ContactUrl)
            },
            License = new OpenApiLicense
            {
                Name = swaggerSettings.LicenseName,
                Url = new Uri("https://www.app-flow.com")
            }
        };
    }

    private static OpenApiSecurityScheme GetOpenApiSecurityScheme()
    {
        return new OpenApiSecurityScheme
        {
            Name = "JWT Authentication",
            Description = "Enter JWT token",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        };
    }

    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
    {
        var swaggerOptions = config.GetSection(nameof(SwaggerOptions)).Get<SwaggerOptions>();

        OpenApiInfo openApiInfo = GetOpenApiInfo(swaggerOptions);

        OpenApiSecurityScheme securityScheme = GetOpenApiSecurityScheme();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerOptions.Version, openApiInfo);

            // Hide the schema
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
            options.DocumentFilter<SwaggerIgnoreFilter>();

            // Add The Jwt Token Option
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, value }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseOpenApiDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.None);
                options.EnablePersistAuthorization();

                options.DefaultModelRendering(ModelRendering.Model);
            });
        }

        return app;
    }

    private sealed class SwaggerIgnoreFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var schema in swaggerDoc.Components.Schemas)
                context.SchemaRepository.Schemas.Remove(schema.Key); // Remove schema from document
        }
    }
}
