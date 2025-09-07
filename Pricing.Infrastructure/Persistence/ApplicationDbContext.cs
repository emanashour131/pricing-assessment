using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pricing.Domain.Entities;

namespace Pricing.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<PriceList> PriceLists => Set<PriceList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Suppliers
        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, Name = "Supplier A", Country = "DE", Active = true, Preferred = true, LeadTimeDays = 5 },
            new Supplier { Id = 2, Name = "Supplier B", Country = "US", Active = true, Preferred = false, LeadTimeDays = 3 },
            new Supplier { Id = 3, Name = "Supplier C", Country = "FR", Active = true, Preferred = false, LeadTimeDays = 4 }
        );

        //  Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Sku = "ABC123", Name = "Product ABC", Uom = "pcs", HazardClass = "None" },
            new Product { Id = 2, Sku = "XYZ777", Name = "Product XYZ", Uom = "pcs", HazardClass = "Flammable" }
        );

        // PriceLists
        modelBuilder.Entity<PriceList>().HasData(
            new PriceList { Id = 1, SupplierId = 1, Sku = "ABC123", ValidFrom = new DateTime(2025, 08, 01), ValidTo = new DateTime(2025, 12, 31), Currency = "EUR", PricePerUom = 9.5m, MinQty = 100 },
            new PriceList { Id = 2, SupplierId = 2, Sku = "ABC123", ValidFrom = new DateTime(2025, 07, 01), ValidTo = new DateTime(2025, 10, 31), Currency = "USD", PricePerUom = 10m, MinQty = 50 },
            new PriceList { Id = 3, SupplierId = 3, Sku = "ABC123", ValidFrom = new DateTime(2025, 08, 15), ValidTo = new DateTime(2025, 12, 31), Currency = "EUR", PricePerUom = 9.5m, MinQty = 100 },

            new PriceList { Id = 4, SupplierId = 1, Sku = "XYZ777", ValidFrom = new DateTime(2025, 08, 01), ValidTo = new DateTime(2025, 12, 31), Currency = "EUR", PricePerUom = 5.25m, MinQty = 10 },
            new PriceList { Id = 5, SupplierId = 2, Sku = "XYZ777", ValidFrom = new DateTime(2025, 09, 01), ValidTo = new DateTime(2025, 11, 30), Currency = "USD", PricePerUom = 5.40m, MinQty = 10 }
        );
    }
}
