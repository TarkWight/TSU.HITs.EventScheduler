using EventScheduler.Domain.Entities;

namespace EventScheduler.Presenters.DTOs
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public string Company { get; set; }
        public ManagerStatus ManagerStatus { get; set; }
    }
}
