using EventScheduler.Domain.Entities;
using EventScheduler.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> UserExists(string email);
        Task<Student> RegisterStudent(Student student, string password);
        Task<Manager> RegisterManager(Manager manager, string password);
        Task<User> Login(string email, string password);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task UpdateUserRefreshToken(User user);
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthRepository(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Students.AnyAsync(u => u.Email == email) ||
                   await _context.Managers.AnyAsync(u => u.Email == email);
        }

        public async Task<Student> RegisterStudent(Student student, string password)
        {
            student.PasswordHash = _passwordHasher.HashPassword(student, password);
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Manager> RegisterManager(Manager manager, string password)
        {
            manager.PasswordHash = _passwordHasher.HashPassword(manager, password);
            manager.Role = UserRole.Manager;
            manager.Status = ManagerStatus.Pending;
            await _context.Managers.AddAsync(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _context.Students.SingleOrDefaultAsync(u => u.Email == email) as User
                        ?? await _context.Managers.SingleOrDefaultAsync(m => m.Email == email) as User;

            if (user == null)
                return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return verificationResult == PasswordVerificationResult.Success ? user : null;
        }

        public async Task UpdateUserRefreshToken(User user)
        {
            if (user is Student student)
            {
                _context.Students.Update(student);
            }
            else if (user is Manager manager)
            {
                _context.Managers.Update(manager);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var hashedRefreshToken = _tokenService.HashRefreshToken(refreshToken);

            var student = await _context.Students
                .FirstOrDefaultAsync(s =>
                    s.RefreshTokenHash != null &&
                    s.RefreshTokenHash == hashedRefreshToken);

            if (student != null)
            {
                return student;
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m =>
                    m.RefreshTokenHash != null &&
                    m.RefreshTokenHash == hashedRefreshToken);

            if (manager != null)
            {
                return manager;
            }

            return null;
        }
    }

}
