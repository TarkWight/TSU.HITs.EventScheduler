using EventScheduler.API.Requests;
using EventScheduler.Domain.Entities;
using EventScheduler.Presenters.DTOs;

namespace EventScheduler.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDTO> RegisterStudentAsync(StudentRegisterRequest request);
        Task<TokenDTO> RegisterManagerAsync(ManagerRegisterRequest request);
        Task<TokenDTO> LoginAsync(LoginRequest request);
        Task<bool> LogoutAsync(RefreshTokenRequest request);
        Task<TokenDTO> RefreshTokenAsync(User user);
    }

}
