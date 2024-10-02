using EventScheduler.Domain.Entities;
using EventScheduler.Presenters.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data.Repositories
{
    public interface IEventRepository
    {
        Task RegisterStudentToEventAsync(Guid eventId, Student student);
        Task<List<Event>> GetEventsByCompanyAsync(Guid companyId);
        Task<List<Event>> GetEventsByStudentAsync(Guid studentId);
        Task<List<Student>> GetStudentsByEventAsync(Guid eventId);
        Task<Event> CreateEventAsync(Event newEvent);
        Task<EventDTO> GetEventDtoAsync(Guid eventId);
        Task<Event> GetEventByIdAsync(Guid eventId);
    }

    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegisterStudentToEventAsync(Guid eventId, Student student)
        {
            var eventEntity = await _context.Events.Include(e => e.RegisteredStudents)
                                                    .FirstOrDefaultAsync(e => e.Id == eventId);
            if (eventEntity != null && eventEntity.RegistrationDeadline > DateTime.Now)
            {
                eventEntity.RegisteredStudents.Add(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Event>> GetEventsByCompanyAsync(Guid companyId)
        {
            return await _context.Events.Where(e => e.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<Event>> GetEventsByStudentAsync(Guid studentId)
        {
            return await _context.Events.Include(e => e.RegisteredStudents)
                                        .Where(e => e.RegisteredStudents.Any(s => s.Id == studentId))
                                        .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByEventAsync(Guid eventId)
        {
            var eventEntity = await _context.Events.Include(e => e.RegisteredStudents)
                                                    .FirstOrDefaultAsync(e => e.Id == eventId);
            return eventEntity?.RegisteredStudents ?? new List<Student>();
        }

        public async Task<Event> CreateEventAsync(Event newEvent)
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<EventDTO> GetEventDtoAsync(Guid eventId)
        {
            var eventEntity = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }

            return new EventDTO
            {
                Title = eventEntity.Title,
                ShortDescription = eventEntity.Description,
                CompanyId = eventEntity.CompanyId,
                ManagerId = eventEntity.ManagerId,
                Location = eventEntity.Location,
                StartDateTime = eventEntity.StartTime,
                EndDateTime = eventEntity.EndTime,
                RegistrationDeadline = eventEntity.RegistrationDeadline
            };
        }


        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }
    }
}
