using Azure.Core;
using EventScheduler.API.Google;
using EventScheduler.Domain.Entities;
using EventScheduler.Domain.Interfaces;
using EventScheduler.Presenters.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace EventScheduler.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // POST: api/events
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "Create a new event", Description = "Creates a new event with the given details.")]
        public async Task<IActionResult> CreateEvent([FromBody] EventDTO eventDto)
        {
            if (eventDto.StartDateTime >= eventDto.EndDateTime)
            {
                return BadRequest("Start time must be before end time.");
            }

            var createdEvent = await _eventService.CreateEventAsync(eventDto);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.CompanyId }, createdEvent);
        }

        // GET: api/events/companies/{companyId}
        [HttpGet("companies/{companyId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EventEntity>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get events by company", Description = "Retrieves a list of events for a specific company by company ID.")]
        public async Task<ActionResult<List<EventEntity>>> GetEventsByCompany(Guid companyId)
        {
            var events = await _eventService.GetEventsByCompanyAsync(companyId);
            return Ok(events);
        }

        // GET: api/events/students/{studentId}
        [HttpGet("students/{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EventEntity>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [SwaggerOperation(Summary = "Get events for a student", Description = "Retrieves a list of events for a specific student by student ID.")]
        public async Task<ActionResult<List<EventEntity>>> GetEventsByStudent(Guid studentId)
        {
            var events = await _eventService.GetEventsByStudentAsync(studentId);
            return Ok(events);
        }

        // GET: api/events/{id}
        [HttpGet("{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get event by ID", Description = "Retrieves event details by event ID.")]
        public async Task<ActionResult<EventDTO>> GetEventById(Guid id)
        {
            var eventDto = await _eventService.GetEventByIdAsync(id);
            return Ok(eventDto);
        }

        // GET: api/events/{id}/students
        [HttpGet("{eventId}/students")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Student>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [SwaggerOperation(Summary = "Get students by event", Description = "Retrieves a list of students registered for a specific event by event ID.")]
        public async Task<ActionResult<List<Student>>> GetStudentsByEvent(Guid id)
        {
            var students = await _eventService.GetStudentsByEventAsync(id);
            return Ok(students);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [SwaggerOperation(Summary = "Register student for event", Description = "Registers a student for a specific event by event ID.")]
        public async Task<IActionResult> RegisterStudent(StudentEventRegisterDTO eventRegisterDTO)
        {
            await _eventService.RegisterStudentToEventAsync(eventRegisterDTO);
            
            return Ok();
        }

    }

}
