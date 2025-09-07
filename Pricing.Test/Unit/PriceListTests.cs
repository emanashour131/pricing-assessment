using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Pricing.Application.DTOs;
using Pricing.Infrastructure.Persistence;
using Pricing.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Test.Unit;

public class PriceListTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
    [Fact]
    public async Task InsertPriceListCSVAsync_AddsPriceLists()
    {
        // Arrange
        var db = GetDbContext();
        var loggerMock = new Mock<ILogger<PriceListRepository>>();
        var repo = new PriceListRepository(db, loggerMock.Object);

        var dtos = new List<PriceListDto>
        {
            new PriceListDto { SupplierId = 1, Sku = "ABC123", Currency = "EUR", PricePerUom = 9.5m, MinQty = 100, ValidFrom = DateTime.Today, ValidTo = DateTime.Today.AddMonths(3) },
            new PriceListDto { SupplierId = 2, Sku = "XYZ777", Currency = "USD", PricePerUom = 10m, MinQty = 50, ValidFrom = DateTime.Today, ValidTo = DateTime.Today.AddMonths(2) }
        };

        // Act
        await repo.InsertPriceListCSVAsync(dtos);

        // Assert
        var all = await db.PriceLists.ToListAsync();
        Assert.Equal(2, all.Count);
        Assert.Contains(all, p => p.Sku == "ABC123" && p.SupplierId == 1);
        Assert.Contains(all, p => p.Sku == "XYZ777" && p.SupplierId == 2);
    }
}
