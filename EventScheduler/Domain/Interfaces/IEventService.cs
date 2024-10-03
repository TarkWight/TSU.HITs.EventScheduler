using EventScheduler.Domain.Entities;
using EventScheduler.Presenters.DTOs;

namespace EventScheduler.Domain.Interfaces
{
    public interface IEventService
    {
        Task RegisterStudentToEventAsync(StudentEventRegisterDTO eventRegisterDTO);
        Task<List<EventEntity>> GetEventsByCompanyAsync(Guid companyId);
        Task<List<EventEntity>> GetEventsByStudentAsync(Guid studentId);
        Task<List<Student>> GetStudentsByEventAsync(Guid eventId);
        Task<EventDTO> CreateEventAsync(EventDTO eventDto);
        Task<EventDTO> GetEventByIdAsync(Guid id);
    }
}
