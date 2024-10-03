using EventScheduler.API.Requests;
using EventScheduler.Data.Repositories;
using EventScheduler.Domain.Entities;
using EventScheduler.Domain.Interfaces;
using EventScheduler.Presenters.DTOs;
using EventScheduler.Security;
using Microsoft.AspNetCore.Identity;

namespace EventScheduler.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(AuthRepository authRepository, ITokenService tokenService, IPasswordHasher<User> passwordHasher)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokenDTO> RegisterStudentAsync(StudentRegisterRequest request)
        {
            if (await _authRepository.UserExists(request.Email))
            {
                return null;
            }

            var student = new Student
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,

            };

            var refreshToken = _tokenService.GenerateRefreshToken();
            student.RefreshTokenHash = _tokenService.HashRefreshToken(refreshToken);
            student.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            student = await _authRepository.RegisterStudent(student, request.Password);

            var accessToken = _tokenService.GenerateToken(student);
            return new TokenDTO
            {
                AccessToken = accessToken,
                AccessTokenExpiration = DateTime.UtcNow.AddHours(1),
                RefreshToken = refreshToken,
                RefreshTokenExpiration = student.RefreshTokenExpiration
            };
        }

        public async Task<TokenDTO> RegisterManagerAsync(ManagerRegisterRequest request)
        {
            if (await _authRepository.UserExists(request.Email))
            {
                return null;
            }

            var manager = new Manager
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                CompanyId = request.idCompany,
                Status = ManagerStatus.Pending,
            };

            var refreshToken = _tokenService.GenerateRefreshToken();
            manager.RefreshTokenHash = _tokenService.HashRefreshToken(refreshToken);
            manager.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            manager = await _authRepository.RegisterManager(manager, request.Password);

            var accessToken = _tokenService.GenerateToken(manager);
            return new TokenDTO
            {
                AccessToken = accessToken,
                AccessTokenExpiration = DateTime.UtcNow.AddHours(1),
                RefreshToken = refreshToken,
                RefreshTokenExpiration = manager.RefreshTokenExpiration
            };
        }


        public async Task<TokenDTO> LoginAsync(LoginRequest request)
        {
            var user = await _authRepository.Login(request.Email, request.Password);

            if (user == null)
            {
                return null;
            }

            var accessToken = _tokenService.GenerateToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenHash = _tokenService.HashRefreshToken(refreshToken);
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            await _authRepository.UpdateUserRefreshToken(user);

            return new TokenDTO
            {
                AccessToken = accessToken,
                AccessTokenExpiration = DateTime.UtcNow.AddHours(1),
                RefreshToken = refreshToken,
                RefreshTokenExpiration = user.RefreshTokenExpiration
            };
        }

        public async Task<bool> LogoutAsync(RefreshTokenRequest request)
        {
            var student = await _authRepository.GetUserByRefreshTokenAsync(request.RefreshToken);

            if (student != null)
            {
                student.RefreshTokenHash = "REVOKED";
                student.RefreshTokenExpiration = DateTime.UtcNow;
                await _authRepository.UpdateUserRefreshToken(student);
                return true;
            }

            var manager = await _authRepository.GetUserByRefreshTokenAsync(request.RefreshToken);

            if (manager != null)
            {
                manager.RefreshTokenHash = "REVOKED";
                manager.RefreshTokenExpiration = DateTime.UtcNow;
                await _authRepository.UpdateUserRefreshToken(manager);
                return true;
            }
            return false;
        }

        public async Task<TokenDTO> RefreshTokenAsync(User user)
        {
            if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
            {
                return null;
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newAccessToken = _tokenService.GenerateToken(user);

            user.RefreshTokenHash = _tokenService.HashRefreshToken(newRefreshToken);
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            await _authRepository.UpdateUserRefreshToken(user);


            return new TokenDTO
            {
                AccessToken = newAccessToken,
                AccessTokenExpiration = DateTime.UtcNow.AddHours(1),
                RefreshToken = newRefreshToken,
                RefreshTokenExpiration = user.RefreshTokenExpiration
            };
        }


    }

}
