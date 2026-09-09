using Microsoft.OpenApi.Models;

namespace NoteApplication.API.Extensions;

public static class ApplicationSwaggerGen
{
    public static void AddApplicationSwaggerGen(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(e =>
        {
            e.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "NoteApplication API",
            });

            e.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your valid token.\r\nExample: \"...\"",
            });

            e.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }
}