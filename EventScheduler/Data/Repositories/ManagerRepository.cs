using EventScheduler.Domain.Entities;
using EventScheduler.Presenters.DTOs;
using EventScheduler.Security;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data.Repositories
{
    public interface IManagerRepository
    {
        Task<List<Manager>> GetAllManagersAsync();
        Task<Manager> GetManagerByIdAsync(Guid id);
        Task<TokenDTO> UpdateManagerStatusAsync(Guid id, ManagerStatus status);
        Task<List<Manager>> GetPendingManagersAsync();
        Task<EventEntity> CreateEventAsync(Guid companyId, EventEntity newEvent);
        Task<Manager> GetByUserIdAsync(Guid userId);
    }

    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        public ManagerRepository(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Manager> GetByUserIdAsync(Guid userId)
        {
            return await _context.Managers
                .FirstOrDefaultAsync(m => m.Id == userId);
        }

        public async Task<List<Manager>> GetAllManagersAsync()
        {
            return await _context.Managers.ToListAsync();
        }

        public async Task<Manager> GetManagerByIdAsync(Guid managerId)
        {
            return await _context.Managers
                .FirstOrDefaultAsync(manager => manager.Id == managerId);
        }

        public async Task<TokenDTO> UpdateManagerStatusAsync(Guid id, ManagerStatus status)
        {
            var manager = await GetManagerByIdAsync(id);

            if (manager != null)
            {
                manager.Status = status;
                await _context.SaveChangesAsync();

                var newAccessToken = _tokenService.GenerateToken(manager);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                Console.WriteLine($"New Access Token: {newAccessToken}");
                manager.RefreshTokenHash = _tokenService.HashRefreshToken(newRefreshToken);
                manager.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

                await _context.SaveChangesAsync();

                return new TokenDTO
                {
                    AccessToken = newAccessToken,
                    AccessTokenExpiration = DateTime.UtcNow.AddHours(1),
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiration = manager.RefreshTokenExpiration
                };
            }

            return null;
        }

        public async Task<List<Manager>> GetPendingManagersAsync()
        {
            return await _context.Managers
                .Where(manager => manager.Status == ManagerStatus.Pending)
                .ToListAsync();
        }

        public async Task<EventEntity> CreateEventAsync(Guid companyId, EventEntity newEvent)
        {
            newEvent.CompanyId = companyId;
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }
    }
}
