using EventScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data.Repositories
{
    public interface IAPIRepository
    {
        Task<User?> GetUserByIdAndRoleAsync(Guid userId);
    }
    public class APIRepository
    {
        private readonly ApplicationDbContext _context;

        public APIRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var dean = await _context.Deans.FirstOrDefaultAsync(d => d.Id == userId);
            if (dean != null)
            {
                return dean;
            }

            var manager = await _context.Managers.FirstOrDefaultAsync(m => m.Id == userId);
            if (manager != null)
            {
                return manager;
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == userId);
            if (student != null)
            {
                return student;
            }

            return null;
        }
    }
}
