using Azure.Core;
using EventScheduler.Domain.Entities;
using EventScheduler.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

[ApiController]
[Route("api/managers")]
public class ManagersController : ControllerBase
{
    private readonly IManagerService _ManagerService;

    public ManagersController(IManagerService ManagerService)
    {
        _ManagerService = ManagerService;
    }

    // GET: api/managers
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Manager>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Dean")]
    [SwaggerOperation(Summary = "Get a list of all company managers", Description = "Returns a list of company managers.")]
    public async Task<ActionResult<List<Manager>>> GetAllManagers()
    {
        var managers = await _ManagerService.GetAllManagersAsync();
        return Ok(managers);
    }


    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Dean")]
    [SwaggerOperation(Summary = "Update the status of a company manager", Description = "Updates the status of a specific company manager based on their ID.")]
    public async Task<IActionResult> UpdateManagerStatus(Guid id, [FromBody] ManagerStatus status)
    {
        await _ManagerService.UpdateManagerStatusAsync(id, status);
        return Ok();
    }

    // GET: api/managers/pending
    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Manager>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Dean")]
    [SwaggerOperation(Summary = "Get pending company managers", Description = "Retrieves a list of company managers awaiting status approval.")]
    public async Task<IActionResult> GetPendingManagers()
    {
        var pendingManagers = await _ManagerService.GetPendingManagersAsync();

        if (pendingManagers == null || !pendingManagers.Any())
        {
            return NotFound("No pending managers found.");
        }

        return Ok(pendingManagers);
    }

    //[HttpPost("company/{companyId}/events")]
    //[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Event))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[Authorize(Roles = "Manager")]
    //[SwaggerOperation(Summary = "Create a new event for a specific company", Description = "Creates a new event under the specified company.")]
    //public async Task<IActionResult> CreateEvent(Guid companyId, [FromBody] EventDTO eventDto)
    //{
    //    if (eventDto == null)
    //    {
    //        return BadRequest("Event data is required.");
    //    }

    //    var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    //    Console.WriteLine($"Manager Access Token: {token}");

    //    var managerIdString = User.Claims
    //        .Where(c => c.Type == ClaimTypes.NameIdentifier)
    //        .Select(c => c.Value)
    //        .LastOrDefault();

    //    foreach (var claim in User.Claims)
    //    {
    //        Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
    //    }


    //    Console.WriteLine($"Manager ID from token: {managerIdString}");

    //    if (string.IsNullOrEmpty(managerIdString) || !Guid.TryParse(managerIdString, out var managerId))
    //    {
    //        Console.WriteLine("Invalid manager identifier. It may be null or empty or not a valid GUID.");
    //        return BadRequest("Invalid manager identifier.");
    //    }

    //    var manager = await _ManagerService.GetManagerByIdAsync(managerId);
    //    if (manager == null)
    //    {
    //        return NotFound("Manager not found.");
    //    }

    //    if (manager.Status != ManagerStatus.Confirmed)
    //    {
    //        return Unauthorized("You are not authorized to create an event for this company.");
    //    }

    //    if (manager.CompanyId != companyId)
    //    {
    //        return Unauthorized("You are not authorized to create an event for this company.");
    //    }

    //    var newEvent = new Event
    //    {
    //        Id = Guid.NewGuid(),
    //        Title = eventDto.Title,
    //        Description = eventDto.ShortDescription,
    //        StartTime = eventDto.StartDateTime,
    //        EndTime = eventDto.EndDateTime,
    //        Location = eventDto.Location,
    //        RegistrationDeadline = eventDto.RegistrationDeadline,
    //        CompanyId = companyId
    //    };

    //    var createdEvent = await _ManagerService.CreateEventAsync(companyId, newEvent, managerId);
    //    return CreatedAtAction(nameof(CreateEvent), new { id = createdEvent.Id }, createdEvent);
    //}





}
