using EventScheduler.API.Requests;
using EventScheduler.Presenters.DTOs;

namespace EventScheduler.Domain.Interfaces
{
    public interface ICompanyService
    {
        Task<CompaniesResponseDTO> GetCompaniesAsync(PaginationRequestDTO paginationDTO);
        Task<CompanyDTO> CreateCompanyAsync(string newCompanyName);
        Task<CompanyDTO> UpdateCompanyAsync(CompanyDTO companyDTO);
        Task DeleteCompanyAsync(Guid id);
    }
}
