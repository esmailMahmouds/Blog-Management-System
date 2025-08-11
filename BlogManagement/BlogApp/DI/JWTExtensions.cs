using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlogApp.DI
{
    public static class JWTExtension
    {
        public static IServiceCollection AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var jwtKey = configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentException("JWT Key is not configured in appsettings.json");
            }

            if (jwtKey.Length < 32)
            {
                throw new ArgumentException("JWT Key must be at least 32 characters for security.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            return services;
        }
    }
}




