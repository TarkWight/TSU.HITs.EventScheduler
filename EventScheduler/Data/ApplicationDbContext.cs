using EventScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Dean> Deans { get; set; }
    }

}
