using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EventScheduler.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid ManagerId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }  
        public DateTime RegistrationDeadline { get; set; }
        public List<Student>? RegisteredStudents { get; set; }
    }
}
