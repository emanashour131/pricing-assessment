using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pricing.Application.DTOs;
using Pricing.Application.Interfaces;
using Pricing.Domain.Entities;
using Pricing.Infrastructure.Persistence;

namespace Pricing.Infrastructure.Repositories;

public class PriceListRepository : IPriceListRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PriceListRepository> _logger;

    public PriceListRepository(ApplicationDbContext context, ILogger<PriceListRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<PriceListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PriceLists
                    .AsNoTracking()
                    .Select(p => new PriceListDto
                    {
                        Id = p.Id,
                        SupplierId = p.SupplierId,
                        Sku = p.Sku,
                        ValidFrom = p.ValidFrom,
                        ValidTo = p.ValidTo,
                        Currency = p.Currency,
                        PricePerUom = p.PricePerUom,
                        MinQty = p.MinQty
                    })
                    .ToListAsync(cancellationToken);
    }

    public async Task InsertPriceListCSVAsync(IEnumerable<PriceListDto> priceListDtos, CancellationToken cancellationToken = default)
    {
        var entities = priceListDtos.Select(dto => new PriceList
        {
            SupplierId = dto.SupplierId,
            Sku = dto.Sku,
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo,
            Currency = dto.Currency,
            PricePerUom = dto.PricePerUom,
            MinQty = dto.MinQty
        }).ToList();

        await _context.PriceLists.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Bulk insert of {Count} PriceLists completed", entities.Count);
    }
}
