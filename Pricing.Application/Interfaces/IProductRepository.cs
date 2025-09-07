using Pricing.Application.DTOs;

namespace Pricing.Application.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateAsync(ProductDto productDto, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateAsync(int id, ProductDto productDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
