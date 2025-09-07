using Pricing.Application.DTOs;

namespace Pricing.Application.Interfaces;

public interface IPriceListRepository
{
    Task<IEnumerable<PriceListDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task InsertPriceListCSVAsync(IEnumerable<PriceListDto> priceListDtos, CancellationToken cancellationToken = default);
}
