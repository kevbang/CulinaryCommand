using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryCommand.Services
{
    public interface ICompanyService
    {
        Task<Company?> GetCompanyByIdAsync(int id);
        Task<Company?> GetCompanyWithLocationsAsync(int id);
        Task<Company> CreateCompanyAsync(Company company);
        Task UpdateCompanyAsync(Company company);
        Task DeleteCompanyAsync(int id);
    }

    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company?> GetCompanyWithLocationsAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.Locations)
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            company.CreatedAt = DateTime.UtcNow;
            company.UpdatedAt = DateTime.UtcNow;

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task UpdateCompanyAsync(Company updated)
        {
            var existing = await _context.Companies.FindAsync(updated.Id);
            if (existing == null) return;

            existing.Name = updated.Name;
            existing.CompanyCode = updated.CompanyCode;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}