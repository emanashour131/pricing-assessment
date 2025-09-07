using Pricing.Application.DTOs;
namespace Pricing.Application.Interfaces;

public interface ISupplierRepository
{
    Task<IEnumerable<SupplierDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<SupplierDto?> GetByIdAsync(int supplierId, CancellationToken cancellationToken);
    Task<SupplierDto> CreateAsync(SupplierDto newCategory, CancellationToken cancellationToken);
    Task<SupplierDto> UpdateAsync(int supplierId, SupplierDto supplier, CancellationToken cancellationToken);
    Task DeleteAsync(int supplierId, CancellationToken cancellationToken);
}
