using EventScheduler.Data.Repositories;
using EventScheduler.Domain.Entities;
using EventScheduler.Domain.Interfaces;

namespace EventScheduler.Domain.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _repository;
        //private readonly IEventRepository _eventRepository;

        public ManagerService(IManagerRepository repository/*, IEventRepository eventRepository*/)
        {
            _repository = repository;
            //_eventRepository = eventRepository;
        }

        public async Task<List<Manager>> GetAllManagersAsync()
        {
            return await _repository.GetAllManagersAsync();
        }

        public async Task UpdateManagerStatusAsync(Guid id, ManagerStatus status)
        {
            await _repository.UpdateManagerStatusAsync(id, status);
        }

        public async Task<List<Manager>> GetPendingManagersAsync()
        {
            return await _repository.GetPendingManagersAsync();
        }

        //public async Task<Event> CreateEventAsync(Guid companyId, Event newEvent, Guid managerId)
        //{
        //    var manager = await _repository.GetManagerByIdAsync(managerId);

        //    if (manager == null || manager.Status != ManagerStatus.Confirmed || manager.CompanyId != companyId)
        //    {
        //        throw new UnauthorizedAccessException("You are not authorized to create an event for this company.");
        //    }

        //    newEvent.CompanyId = companyId;
        //    return await _eventRepository.CreateEventAsync(newEvent);
        //}

        public async Task<Manager> GetManagerByIdAsync(Guid managerId)
        {
            return await _repository.GetManagerByIdAsync(managerId);
        }
    }

}
