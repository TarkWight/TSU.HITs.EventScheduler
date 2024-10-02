using EventScheduler.API.Requests;
using EventScheduler.Data.Repositories;
using EventScheduler.Domain.Interfaces;
using EventScheduler.Presenters.DTOs;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventScheduler.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthService authService, IAuthRepository authRepository)
        {
            _authService = authService;
            _authRepository = authRepository;
        }

        // POST: api/auth/register/student
        [HttpPost("register/student")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Register a new student", Description = "Registers a new student and returns a JWT token.")]
        public async Task<IActionResult> RegisterStudent([FromBody] StudentRegisterRequest request)
        {
            var result = await _authService.RegisterStudentAsync(request);
            if (result == null)
            {
                return BadRequest("Registration failed.");
            }
            return Ok(result);
        }

        // POST: api/auth/register/manager
        [HttpPost("register/manager")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Register a new company manager", Description = "Registers a new manager for a company and returns a JWT token.")]
        public async Task<IActionResult> RegisterManager([FromBody]ManagerRegisterRequest request)
        {
            var result = await _authService.RegisterManagerAsync(request);
            if (result == null)
            {
                return BadRequest("Registration failed.");
            }
            return Ok(result);
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Login a user", Description = "Logs in a user and returns a JWT token.")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
            {
                return BadRequest("Login failed.");
            }
            return Ok(result);
        }

        // Delete: api/auth/logout
        [HttpDelete("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Logout a user", Description = "Logs out a user and invalidates the JWT token.")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            var success = await _authService.LogoutAsync(request);
            if (!success)
            {
                return BadRequest("Logout failed.");
            }
            return NoContent();
        }

        // POST: api/auth/refresh
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
            Summary = "Refresh JWT token",
            Description = "Refreshes the JWT token using the provided refresh token. Requires a valid refresh token and returns a new JWT token if successful.",
            OperationId = "RefreshToken"
        )]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = await _authRepository.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _authService.RefreshTokenAsync(user);
            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }

    }
}
