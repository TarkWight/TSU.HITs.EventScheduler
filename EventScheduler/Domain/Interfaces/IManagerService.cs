using EventScheduler.Domain.Entities;

namespace EventScheduler.Domain.Interfaces
{
    public interface IManagerService
    {
        Task<List<Manager>> GetAllManagersAsync();
        Task UpdateManagerStatusAsync(Guid id, ManagerStatus status);
        Task<List<Manager>> GetPendingManagersAsync();
        //Task<Event> CreateEventAsync(Guid companyId, Event newEvent, Guid managerId);
        Task<Manager> GetManagerByIdAsync(Guid managerId);
    }
}
