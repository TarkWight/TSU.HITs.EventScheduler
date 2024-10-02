namespace EventScheduler.Presenters.DTOs
{
    public class EventDTO
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public Guid CompanyId { get; set; }
        public Guid ManagerId { get; set; }
        public string Location { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime RegistrationDeadline { get; set; }

    }
}
