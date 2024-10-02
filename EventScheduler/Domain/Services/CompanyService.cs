using EventScheduler.API.Requests;
using EventScheduler.Data.Repositories;
using EventScheduler.Domain.Entities;
using EventScheduler.Domain.Interfaces;
using EventScheduler.Presenters.DTOs;

namespace EventScheduler.Domain.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompaniesResponseDTO> GetCompaniesAsync(PaginationRequestDTO paginationDTO)
        {
            var page = paginationDTO.Page ?? 1;
            var size = paginationDTO.Size ?? 5;

            var companies = await _companyRepository.GetCompaniesAsync(page, size);
            var totalCount = await _companyRepository.GetTotalCompanyCountAsync();

            var companiesDTO = companies.Select(c => new CompanyDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            var pagination = new PaginationDTO
            {
                Size = size,
                Count = totalCount,
                Current = page
            };

            return new CompaniesResponseDTO
            {
                Companies = companiesDTO,
                Pagination = pagination
            };
        }

        public async Task<CompanyDTO> CreateCompanyAsync(string newCompanyName)
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = newCompanyName
            };

            var createdCompany = await _companyRepository.AddCompanyAsync(company);
            return new CompanyDTO
            {
                Id = createdCompany.Id,
                Name = createdCompany.Name
            };
        }

        public async Task<CompanyDTO> UpdateCompanyAsync(CompanyDTO companyDTO)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(companyDTO.Id);
            if (company == null)
            {
                throw new KeyNotFoundException("Company not found");
            }

            company.Name = companyDTO.Name;

            var updatedCompany = await _companyRepository.UpdateCompanyAsync(company);
            return new CompanyDTO
            {
                Id = updatedCompany.Id,
                Name = updatedCompany.Name
            };
        }

        public async Task DeleteCompanyAsync(Guid id)
        {
            await _companyRepository.DeleteCompanyAsync(id);
        }
    }
}
