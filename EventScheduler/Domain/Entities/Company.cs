namespace EventScheduler.Domain.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Manager>? Managers { get; set; }
        public List<Event>? Events { get; set; }
    }
}
