using EventScheduler.Domain.Entities;
using EventScheduler.Presenters.DTOs;

namespace EventScheduler.Domain.Interfaces
{
    public interface IEventService
    {
        //Task RegisterStudentToEventAsync(Guid eventId, Student student);
        Task<List<Event>> GetEventsByCompanyAsync(Guid companyId);
        Task<List<Event>> GetEventsByStudentAsync(Guid studentId);
        Task<List<Student>> GetStudentsByEventAsync(Guid eventId);
        Task<EventDTO> CreateEventAsync(EventDTO eventDto);
        Task<EventDTO> GetEventByIdAsync(Guid id);
    }
}
