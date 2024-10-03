using EventScheduler.Domain.Entities;
using EventScheduler.Presenters.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data.Repositories
{
    public interface IEventRepository
    {
        Task RegisterStudentToEventAsync(StudentEventRegisterDTO eventRegisterDTO);
        Task<List<EventEntity>> GetEventsByCompanyAsync(Guid companyId);
        Task<List<EventEntity>> GetEventsByStudentAsync(Guid studentId);
        Task<List<Student>> GetStudentsByEventAsync(Guid eventId);
        Task<EventEntity> CreateEventAsync(EventEntity newEvent);
        Task<EventDTO> GetEventDtoAsync(Guid eventId);
        Task<EventEntity> GetEventByIdAsync(Guid eventId);
    }

    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegisterStudentToEventAsync(StudentEventRegisterDTO eventRegisterDTO)
        {
            var eventEntity = await _context.Events.Include(e => e.RegisteredStudents)
                                                    .FirstOrDefaultAsync(e => e.Id == eventRegisterDTO.eventId);
            if (eventEntity != null && eventEntity.RegistrationDeadline > DateTime.Now)
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == eventRegisterDTO.studentId);
                if (student != null)
                {
                    eventEntity.RegisteredStudents.Add(student);
                    await _context.SaveChangesAsync();
                }

            }
        }

        public async Task<List<EventEntity>> GetEventsByCompanyAsync(Guid companyId)
        {
            return await _context.Events.Where(e => e.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<EventEntity>> GetEventsByStudentAsync(Guid studentId)
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

        public async Task<EventEntity> CreateEventAsync(EventEntity newEvent)
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
                throw new KeyNotFoundException("EventEntity not found.");
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


        public async Task<EventEntity> GetEventByIdAsync(Guid eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }
    }
}
