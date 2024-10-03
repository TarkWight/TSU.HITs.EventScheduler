

namespace EventScheduler.Domain.Entities
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }

    public class Student : User
    {
        public List<EventEntity>? RegisteredEvents { get; set; }
    }

    public class Manager : User
    {
        public ManagerStatus Status { get; set; }
        public Guid CompanyId { get; set; }
        public List<EventEntity>? ManagedEvents { get; set; }
    }

    public class Dean : User
    {
    }

    public enum UserRole
    {
        Student,
        Manager,
        Dean
    }

    public enum ManagerStatus
    {
        Pending,
        Confirmed,
        Rejected
    }
}
