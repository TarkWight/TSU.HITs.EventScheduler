using EventScheduler.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventScheduler.Security
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        ClaimsPrincipal ValidateToken(string token);
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
    }
    public class TokenService : ITokenService
    {
        private readonly TokenConfigurations _tokenConfigurations;

        public TokenService()
        {
            _tokenConfigurations = new TokenConfigurations();
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("user_id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            if (user is Manager manager)
            {
                claims.Add(new Claim("ManagerStatus", manager.Status.ToString()));
                claims.Add(new Claim("CompanyId", manager.CompanyId.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(TokenConfigurations.Lifetime),
                Issuer = TokenConfigurations.Issuer,
                Audience = TokenConfigurations.Audience,
                SigningCredentials = new SigningCredentials(TokenConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string HashRefreshToken(string refreshToken)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = TokenConfigurations.Issuer,
                ValidAudience = TokenConfigurations.Audience,
                IssuerSigningKey = TokenConfigurations.GetSymmetricSecurityKey()
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string GenerateManagerToken(Manager manager)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, manager.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("user_id", manager.Id.ToString()),
            new Claim(ClaimTypes.Role, "Manager")
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(TokenConfigurations.Lifetime),
                Issuer = TokenConfigurations.Issuer,
                Audience = TokenConfigurations.Audience,
                SigningCredentials = new SigningCredentials(TokenConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
