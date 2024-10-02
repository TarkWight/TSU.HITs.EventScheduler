using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventScheduler.Security
{
    internal class TokenConfigurations
    {
        public const string Issuer = "TSUHITs";
        public const string Audience = "User";
        private const string Key = "mYGh8lG8d6W7wC1cK2fR3sT4aP5eN9dE0dK8iS6eY3";
        public const int Lifetime = 60;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
