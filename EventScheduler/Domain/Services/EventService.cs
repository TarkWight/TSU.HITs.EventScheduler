using EventScheduler.Data.Repositories;
using EventScheduler.Domain.Entities;
using EventScheduler.Domain.Interfaces;
using EventScheduler.Presenters.DTOs;
using System.ComponentModel.Design;

namespace EventScheduler.Domain.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        //private readonly IGoogleCalendarService _googleCalendarService;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
            //_googleCalendarService = googleCalendarService;
        }

        //public async Task RegisterStudentToEventAsync(Guid eventId, Student student)
        //{
        //    await _eventRepository.RegisterStudentToEventAsync(eventId, student);

        //    var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);

        //    var googleCalendarEvent = new GoogleCalendar
        //    {
        //        Summary = eventEntity.Title,
        //        Description = eventEntity.Description,
        //        Location = eventEntity.Location,
        //        Start = eventEntity.StartTime,
        //        End = eventEntity.EndTime
        //    };

        //    await GoogleCalendarHelper.CreateGoogleCalendar(googleCalendarEvent);
        //}



        public async Task<EventDTO> CreateEventAsync(EventDTO eventDto)
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = eventDto.Title,
                Description = eventDto.ShortDescription,
                CompanyId = eventDto.CompanyId,
                ManagerId = eventDto.ManagerId,
                Location = eventDto.Location,
                StartTime = eventDto.StartDateTime,
                EndTime = eventDto.EndDateTime,
                RegistrationDeadline = eventDto.RegistrationDeadline
            };
           
            await _eventRepository.CreateEventAsync(newEvent);

            return eventDto;
        }

        public async Task<List<Event>> GetEventsByCompanyAsync(Guid companyId)
        {
            return await _eventRepository.GetEventsByCompanyAsync(companyId);
        }

        public async Task<List<Event>> GetEventsByStudentAsync(Guid studentId)
        {
            return await _eventRepository.GetEventsByStudentAsync(studentId);
        }

        public async Task<List<Student>> GetStudentsByEventAsync(Guid eventId)
        {
            return await _eventRepository.GetStudentsByEventAsync(eventId);
        }

        public async Task<EventDTO> GetEventByIdAsync(Guid id)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(id);
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
    }
}
