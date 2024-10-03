using EventScheduler.Domain.Entities;

namespace EventScheduler.Domain.Interfaces
{
    public interface IManagerService
    {
        Task<List<Manager>> GetAllManagersAsync();
        Task UpdateManagerStatusAsync(Guid id, ManagerStatus status);
        Task<List<Manager>> GetPendingManagersAsync();
        Task<EventEntity> CreateEventAsync(Guid companyId, EventEntity newEvent, Guid managerId);
        Task<Manager> GetManagerByIdAsync(Guid managerId);
    }
}
