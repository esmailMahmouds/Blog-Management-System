using BlogApp.Models.DomainClasses;
using BlogApp.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Services.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Name),
                new ("sub", user.Id.ToString()),
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Role, user.Role.ToString()),
                new (ClaimTypes.Email, user.Email)
            };

            var keyString = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(keyString))
            {
                throw new ArgumentException("JWT key is not configured in appsettings.json");
            }

            if (keyString.Length < 32)
            {
                throw new ArgumentException("JWT key must be at least 32 characters long");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var durationStr = _config["Jwt:DurationInMinutes"];

            if (!double.TryParse(durationStr, out double durationInMinutes))
            {
                throw new ArgumentException("JWT duration is not a valid number in appsettings.json");
            }

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(durationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token cannot be null or empty");
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new ArgumentException("Invalid token or user ID claim not found");
            }
            return userId;
        }
    }
}
