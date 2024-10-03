using EventScheduler.Domain.Entities;
using EventScheduler.Security;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data
{
    public static class DatabaseInitializer
    {
        public static async Task<string> Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();

            if (await context.Deans.AnyAsync())
            {
                var dean = await context.Deans.FirstOrDefaultAsync();
                var accessToken = new TokenService().GenerateToken(dean);
                return accessToken;
            }

            var tokenService = new TokenService();
            var refreshToken = tokenService.GenerateRefreshToken();

            var newDean = new Dean
            {
                Id = Guid.NewGuid(),
                Name = "Default Dean",
                Email = "dean@gmail.com",
                Role = UserRole.Dean,
                PasswordHash = HashingHelper.HashPassword("Rjnjdfcbz1@"),
                RefreshTokenHash = tokenService.HashRefreshToken(refreshToken),
                RefreshTokenExpiration = DateTime.UtcNow.AddMonths(1)
            };

            context.Deans.Add(newDean);
            await context.SaveChangesAsync();

            var accessTokenNew = tokenService.GenerateToken(newDean);
            return accessTokenNew;
        }
    }

}
