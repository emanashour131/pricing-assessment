using Pricing.Application.DTOs;

namespace Pricing.Application.Interfaces;

public interface IPriceCalculator
{
    Task<BestPriceDto> GetBestPriceAsync(string sku, int qty, string currency, DateTime date, CancellationToken ct);
}
