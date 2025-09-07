using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pricing.Application.DTOs;
using Pricing.Application.Interfaces;
using Pricing.Domain.Entities;
using Pricing.Infrastructure.Persistence;

namespace Pricing.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ApplicationDbContext applicationDbContext, ILogger<ProductRepository> logger)
    {
        _context = applicationDbContext;
        _logger = logger;
    }

    public async Task<ProductDto> CreateAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Sku = productDto.Sku,
            Name = productDto.Name,
            Uom = productDto.Uom,
            HazardClass = productDto.HazardClass
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        productDto.Id = product.Id;
        _logger.LogInformation("Product {ProductId} created successfully", product.Id);
        return productDto;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (product == null)
        {
            _logger.LogError($"Delete failed. Product {id} not found");
            throw new KeyNotFoundException($"Product with id {id} was not found");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Product {id} deleted successfully");
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.AsNoTracking()
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Uom = p.Uom,
                HazardClass = p.HazardClass
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product=  await _context.Products
                            .AsNoTracking()
                            .Select(p => new ProductDto
                            {
                                Id = p.Id,
                                Sku = p.Sku,
                                Name = p.Name,
                                Uom = p.Uom,
                                HazardClass = p.HazardClass
                            })
                            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (product == null)
        {
            _logger.LogWarning("Product {ProductId} not found", id);
            throw new KeyNotFoundException($"Product with id {id} not found");
        }

        return product;
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (product == null)
        {
            _logger.LogError($"Update failed. Product {id} not found");
            throw new KeyNotFoundException($"Product with id {id} not found");
        }

        product.Sku = productDto.Sku;
        product.Name = productDto.Name;
        product.Uom = productDto.Uom;
        product.HazardClass = productDto.HazardClass;

        _context.Products.Update(product);

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Product {ProductId} updated successfully", product.Id);

        return new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Uom = product.Uom,
            HazardClass = product.HazardClass
        };
    }
}
