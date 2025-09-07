using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pricing.Application.DTOs;
using Pricing.Application.Interfaces;

namespace Pricing.Infrastructure.Repositories;

public class PriceCalculator : IPriceCalculator
{
    private readonly IPriceListRepository _priceListRepo;
    private readonly ISupplierRepository _supplierRepo;
    private readonly IRateProvider _rateProvider;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PriceCalculator> _logger;

    public PriceCalculator(
        IPriceListRepository priceListRepo,
        ISupplierRepository supplierRepo,
        IRateProvider rateProvider,
        IMemoryCache cache,
        ILogger<PriceCalculator> logger)
    {
        _priceListRepo = priceListRepo;
        _supplierRepo = supplierRepo;
        _rateProvider = rateProvider;
        _cache = cache;
        _logger = logger;
    }

    public async Task<BestPriceDto> GetBestPriceAsync(string sku, int qty, string currency, DateTime date, CancellationToken ct)
    {
        var cacheKey = $"bestprice:{sku}:{qty}:{currency}:{date:yyyyMMdd}";
        if (_cache.TryGetValue(cacheKey, out BestPriceDto? cachedResult))
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedResult;
        }

        var prices = await _priceListRepo.GetAllAsync(ct);

        var candidates = prices
            .Where(p => p.Sku == sku &&
                        p.ValidFrom <= date &&
                        p.ValidTo >= date &&
                        p.MinQty <= qty)
            .ToList();

        if (!candidates.Any())
            return null;

        var suppliers = await _supplierRepo.GetAllAsync(ct);

        var converted = candidates
            .Select(p =>
            {
                var unitPrice = _rateProvider.Convert(p.Currency, currency, p.PricePerUom);
                var supplier = suppliers.First(s => s.Id == p.SupplierId);
                return new
                {
                    Supplier = supplier,
                    UnitPrice = unitPrice,
                    Total = unitPrice * qty
                };
            })
            .ToList();

        var best = converted
            .OrderBy(c => c.UnitPrice)
            .ThenByDescending(c => c.Supplier.Preferred)
            .ThenBy(c => c.Supplier.LeadTimeDays)
            .ThenBy(c => c.Supplier.Id)
            .First();

        var result = new BestPriceDto
        {
            SupplierId = best.Supplier.Id,
            SupplierName = best.Supplier.Name,
            UnitPrice = best.UnitPrice,
            Total = best.Total,
            Reasoning = "Lowest price with tie-breakers applied"
        };

        _cache.Set(cacheKey, result, TimeSpan.FromSeconds(30));

        return result;
    }
}
