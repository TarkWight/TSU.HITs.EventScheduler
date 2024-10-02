namespace EventScheduler.Presenters.DTOs
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class CompaniesResponseDTO
    {
        public List<CompanyDTO> Companies { get; set; }
        public PaginationDTO Pagination { get; set; }
    }
}
