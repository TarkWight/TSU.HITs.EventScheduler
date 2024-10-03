using Azure.Core;
using EventScheduler.API.Google;
using EventScheduler.Data.Repositories;
using EventScheduler.Domain.Entities;
using EventScheduler.Domain.Interfaces;
using EventScheduler.Presenters.DTOs;
using Google.Apis.Calendar.v3.Data;
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

        public async Task RegisterStudentToEventAsync(StudentEventRegisterDTO eventRegisterDTO)
        {
            await _eventRepository.RegisterStudentToEventAsync(eventRegisterDTO);

            var eventEntity = await _eventRepository.GetEventByIdAsync(eventRegisterDTO.eventId);


            GoogleCalendar newEvent = new GoogleCalendar()
            {
                Summary = eventEntity.Title,
                Location = eventEntity.Location,
                Description = eventEntity.Description,
                Start = eventEntity.StartTime,
                End = eventEntity.EndTime
            };

            await GoogleCalendarHelper.CreateGoogleCalendar(newEvent);
        }



        public async Task<EventDTO> CreateEventAsync(EventDTO eventDto)
        {
            var newEvent = new EventEntity
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

        public async Task<List<EventEntity>> GetEventsByCompanyAsync(Guid companyId)
        {
            return await _eventRepository.GetEventsByCompanyAsync(companyId);
        }

        public async Task<List<EventEntity>> GetEventsByStudentAsync(Guid studentId)
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
    }
}
