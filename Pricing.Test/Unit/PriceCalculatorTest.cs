using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Pricing.Application.DTOs;
using Pricing.Application.Interfaces;
using Pricing.Domain.Entities;
using Pricing.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Test.Unit;

public class PriceCalculatorTest
{
    [Fact]
    public async Task GetBestPriceAsync_ReturnsLowestPriceWithTieBreakers()
    {
        var priceListMock = new Mock<IPriceListRepository>();
        var supplierMock = new Mock<ISupplierRepository>();
        var rateProviderMock = new Mock<IRateProvider>();

        var date = new DateTime(2025, 09, 01);
        var qty = 120;
        var currency = "EUR";

        supplierMock.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SupplierDto>
            {
                new SupplierDto { Id = 1, Name = "Supplier1", Preferred = false, LeadTimeDays = 5 },
                new SupplierDto { Id = 2, Name = "Supplier2", Preferred = false, LeadTimeDays = 5 },
                new SupplierDto { Id = 3, Name = "Supplier3", Preferred = false, LeadTimeDays = 3 }
            });

        priceListMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PriceListDto>
            {
                new PriceListDto { SupplierId = 1, Sku = "ABC123", Currency = "EUR", PricePerUom = 9.5m, MinQty = 100, ValidFrom = new DateTime(2025,8,1), ValidTo = new DateTime(2025,12,31) },
                new PriceListDto { SupplierId = 2, Sku = "ABC123", Currency = "USD", PricePerUom = 10m, MinQty = 50, ValidFrom = new DateTime(2025,7,1), ValidTo = new DateTime(2025,10,31) },
                new PriceListDto { SupplierId = 3, Sku = "ABC123", Currency = "EUR", PricePerUom = 9.5m, MinQty = 100, ValidFrom = new DateTime(2025,8,15), ValidTo = new DateTime(2025,12,31) }
            });

        rateProviderMock.Setup(rp => rp.Convert("USD", "EUR", 10m)).Returns(9.2m);
        rateProviderMock.Setup(rp => rp.Convert("EUR", "EUR", 9.5m)).Returns(9.5m);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var logger = new Mock<ILogger<PriceCalculator>>().Object;

        var calculator = new PriceCalculator(priceListMock.Object, supplierMock.Object, rateProviderMock.Object, memoryCache, logger);

        // Act
        var result = await calculator.GetBestPriceAsync("ABC123", qty, currency, date, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        dynamic r = result!;
        Assert.Equal(2, (int)r.SupplierId); 
        Assert.Equal(1104m, (decimal)r.Total);
    }
}