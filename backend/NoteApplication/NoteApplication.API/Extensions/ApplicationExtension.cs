using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NoteApplication.API.Common;
using NoteApplication.API.Contexts;
using NoteApplication.API.Features.Auth.Model;
using NoteApplication.API.Features.Auth.Service;
using NoteApplication.API.Features.Notes.Service;
using NoteApplication.API.Repositories;

namespace NoteApplication.API.Extensions;

public static class ApplicationExtension
{
    public static void AddApplicationServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        
        builder.Services.AddSingleton<IAppDataContext>(new AppDataContext(connectionString));

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<INoteService, NoteService>();
        
        builder.Services.AddHttpContextAccessor();

        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        builder.Services.AddAuthorization();
    } 
}