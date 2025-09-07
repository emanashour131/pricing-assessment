using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pricing.Domain.Entities;
using Pricing.Infrastructure.Persistence;
using Pricing.Infrastructure.Repositories;

namespace Pricing.Test.Integration;

public class PriceCalculatorIntegrationTest
{
    private ApplicationDbContext GetInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        var context = new ApplicationDbContext(options);

        context.Suppliers.AddRange(
            new Supplier { Id = 1, Name = "Supplier1", Preferred = false, LeadTimeDays = 5 },
            new Supplier { Id = 2, Name = "Supplier2", Preferred = false, LeadTimeDays = 5 }
        );

        context.Products.Add(new Product { Id = 1, Sku = "ABC123", Name = "Product ABC", Uom = "pcs" });

        context.PriceLists.AddRange(
            new PriceList { SupplierId = 1, Sku = "ABC123", Currency = "EUR", PricePerUom = 9.5m, MinQty = 100, ValidFrom = new DateTime(2025, 8, 1), ValidTo = new DateTime(2025, 12, 31) },
            new PriceList { SupplierId = 2, Sku = "ABC123", Currency = "USD", PricePerUom = 10m, MinQty = 50, ValidFrom = new DateTime(2025, 7, 1), ValidTo = new DateTime(2025, 10, 31) }
        );

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task BestPriceIntegration_ReturnsCorrectSupplier()
    {
        var db = GetInMemoryDb();
        var logger = new LoggerFactory().CreateLogger<PriceCalculator>();
        var priceLogger = new LoggerFactory().CreateLogger<PriceListRepository>();
        var supplierLogger = new LoggerFactory().CreateLogger<SupplierRepository>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var priceRepo = new PriceListRepository(db, priceLogger);
        var supplierRepo = new SupplierRepository(db, supplierLogger);
        var rateProvider = new RateProvider();

        var calculator = new PriceCalculator(priceRepo, supplierRepo, rateProvider, cache, logger);

        var result = await calculator.GetBestPriceAsync("ABC123", 120, "EUR", new DateTime(2025, 09, 01), default);

        Assert.NotNull(result);
        dynamic r = result!;
        Assert.Equal(2, (int)r.SupplierId);
    }
}