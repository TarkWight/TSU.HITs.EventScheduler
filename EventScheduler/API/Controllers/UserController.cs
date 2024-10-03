using EventScheduler.Data.Repositories;
using EventScheduler.Presenters.DTOs;
using EventScheduler.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventScheduler.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IAPIRepository _apiRepository;
        private readonly ICompanyRepository _companyRepository;
        public UserController(IAPIRepository apiRepository, ICompanyRepository companyRepository)
        {
            _apiRepository = apiRepository;
            _companyRepository = companyRepository;
        }

        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult<UserInfoDTO>> GetUserInfo()
        {
            var claims = User.Claims.ToList();
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user identifier format." + userIdClaim.ToString());
            }

            if (User.IsInRole("Manager"))
            {
                Manager manager = (Manager)await _apiRepository.GetUserByIdAsync(userId);
                if (manager == null)
                {
                    return NotFound("Manager not found.");
                }

                var company = await _companyRepository.GetByIdAsync(manager.CompanyId);
                var userInfo = new UserInfoDTO
                {
                    Id = manager.Id,
                    Name = manager.Name,
                    Role = manager.Role,
                    Company = company?.Name,
                    ManagerStatus = manager.Status
                };

                return Ok(userInfo);
            }
            else if (User.IsInRole("Student"))
            {
                var student = await _apiRepository.GetUserByIdAsync(userId);
                if (student == null)
                {
                    return NotFound("Student not found.");
                }

                var userInfo = new UserInfoDTO
                {
                    Id = student.Id,
                    Name = student.Name,
                    Role = student.Role
                };

                return Ok(userInfo);
            }
            else if (User.IsInRole("Dean"))
            {
                var dean = await _apiRepository.GetUserByIdAsync(userId);
                if (dean == null)
                {
                    return NotFound("Dean not found.");
                }

                var userInfo = new UserInfoDTO
                {
                    Id = dean.Id,
                    Name = dean.Name,
                    Role = dean.Role
                };

                return Ok(userInfo);
            }

            return NotFound("User role not recognized.");
        }
    }
}
