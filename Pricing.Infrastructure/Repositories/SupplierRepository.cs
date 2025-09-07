using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pricing.Application.DTOs;
using Pricing.Application.Interfaces;
using Pricing.Domain.Entities;
using Pricing.Infrastructure.Persistence;

namespace Pricing.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SupplierRepository> _logger;

    public SupplierRepository(ApplicationDbContext applicationDbContext, ILogger<SupplierRepository> logger)
    {
        _context = applicationDbContext;
        _logger = logger;
    }

    public async Task<SupplierDto> CreateAsync(SupplierDto supplierDto, CancellationToken cancellationToken = default)
    {
        var supplier = new Supplier
        {
            Name = supplierDto.Name,
            Country = supplierDto.Country,
            Active = supplierDto.Active,
            Preferred = supplierDto.Preferred,
            LeadTimeDays = supplierDto.LeadTimeDays
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync(cancellationToken);

        supplierDto.Id = supplier.Id;
        _logger.LogInformation($"Supplier {supplier.Id} created successfully");

        return supplierDto;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        Supplier? supplierFromDb = await _context.Suppliers.FirstOrDefaultAsync(e => e.Id == id);
        if (supplierFromDb == null)
        {
            _logger.LogError($"Delete failed. Supplier {id} not found");
            throw new KeyNotFoundException($"{nameof(supplierFromDb)} was not found");
        }

        _context.Suppliers.Remove(supplierFromDb);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Product {id} deleted successfully");
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers.AsNoTracking().Select(s => new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            Country = s.Country,
            Active = s.Active,
            Preferred = s.Preferred,
            LeadTimeDays = s.LeadTimeDays
        }).ToListAsync(cancellationToken);
    }

    public async Task<SupplierDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var supplier = await _context.Suppliers.AsNoTracking().Select(s => new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            Country = s.Country,
            Active = s.Active,
            Preferred = s.Preferred,
            LeadTimeDays = s.LeadTimeDays
        }).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (supplier == null)
        {
            _logger.LogWarning("supplier {supplier} not found", id);
            throw new KeyNotFoundException($"supplier with id {id} not found");
        }

        return supplier;
    }

    public async Task<SupplierDto> UpdateAsync(int id, SupplierDto supplierDto, CancellationToken cancellationToken)
    {
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);

        if (supplier == null)
        {
            _logger.LogError($"Update failed. Supplier {id} not found");
            throw new KeyNotFoundException($"Supplier with id {id} not found");
        }

        supplier.Name = supplierDto.Name;
        supplier.Country = supplierDto.Country;
        supplier.Active = supplierDto.Active;
        supplier.Preferred = supplierDto.Preferred;
        supplier.LeadTimeDays = supplierDto.LeadTimeDays;

        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync(cancellationToken);

        return new SupplierDto { Id = supplier.Id, Name = supplier.Name, Country = supplier.Country, Active = supplier.Active, Preferred = supplier.Preferred, LeadTimeDays = supplier.LeadTimeDays };
    }
}
