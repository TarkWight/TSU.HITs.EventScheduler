using EventScheduler.API.Requests;
using EventScheduler.Domain.Interfaces;
using EventScheduler.Presenters.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventScheduler.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        // GET: api/company/list
        [HttpGet("list")]
        [ProducesResponseType(typeof(CompaniesResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get list of companies", Description = "Returns a list of companies with their IDs and names.")]
        public async Task<IActionResult> GetCompanies([FromQuery] PaginationRequestDTO paginationDTO)
        {
            try
            {
                var result = await _companyService.GetCompaniesAsync(paginationDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting companies");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/company
        [Authorize(Roles = "Dean")]
        [HttpPost]
        [ProducesResponseType(typeof(String), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create a new company", Description = "Creates a new company. Available only to Deans.")]
        public async Task<IActionResult> CreateCompany([FromBody] string newCompanyName)
        {
            try
            {
                var createdCompany = await _companyService.CreateCompanyAsync(newCompanyName);
                return CreatedAtAction(nameof(GetCompanies), new { id = createdCompany.Id }, createdCompany);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating company");
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

        // PUT: api/company/{id}
        [Authorize(Roles = "Dean")]
        [HttpPut]
        [ProducesResponseType(typeof(CompanyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Update a company", Description = "Updates a company's details. Available only to Deans.")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyDTO companyDTO)
        {
            try
            {
                var updatedCompany = await _companyService.UpdateCompanyAsync(companyDTO);
                return Ok(updatedCompany);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Company not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating company");
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

        // DELETE: api/company/{id}
        [Authorize(Roles = "Dean")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Delete a company", Description = "Deletes a company. Available only to Deans.")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            try
            {
                await _companyService.DeleteCompanyAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Company not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting company");
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }
    }
}
