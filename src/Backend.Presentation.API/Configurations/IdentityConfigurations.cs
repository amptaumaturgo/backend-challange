using Backend.Infrastructure;
using Backend.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Backend.Presentation.API.Configurations;

public static class IdentityConfiguration
{
    public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationSettingsSection = configuration.GetSection("AuthenticationSettings");
        services.Configure<AuthenticationSettings>(authenticationSettingsSection);

        var authenticationSettings = authenticationSettingsSection.Get<AuthenticationSettings>();

        var key = Encoding.ASCII.GetBytes(authenticationSettings!.Secret);

        services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                IssuerSigningKey =  new SymmetricSecurityKey(key), 
                ValidateIssuer = false 
            };
        }); 
    }
}