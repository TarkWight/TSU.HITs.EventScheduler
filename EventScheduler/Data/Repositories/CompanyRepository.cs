using EventScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data.Repositories
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetCompaniesAsync(int page, int size);
        Task<int> GetTotalCompanyCountAsync();
        Task<Company> GetCompanyByIdAsync(Guid id);
        Task<Company> AddCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(Company company);
        Task DeleteCompanyAsync(Guid id);
    }

    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Company>> GetCompaniesAsync(int page, int size)
        {
            return await _dbContext.Companies
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetTotalCompanyCountAsync()
        {
            return await _dbContext.Companies.CountAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(Guid id)
        {
            return await _dbContext.Companies.FindAsync(id);
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            _dbContext.Companies.Add(company);
            await _dbContext.SaveChangesAsync();
            return company;
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            _dbContext.Companies.Update(company);
            await _dbContext.SaveChangesAsync();
            return company;
        }

        public async Task DeleteCompanyAsync(Guid id)
        {
            var company = await _dbContext.Companies.FindAsync(id);
            if (company != null)
            {
                _dbContext.Companies.Remove(company);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
