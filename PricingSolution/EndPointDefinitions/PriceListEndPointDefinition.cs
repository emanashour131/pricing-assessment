using CsvHelper;
using CsvHelper.Configuration;
using Pricing.Application.DTOs;
using Pricing.Application.Interfaces;
using System.Globalization;

namespace Pricing.Api.EndPointDefinitions;

public class PriceListPointDefinition : IEndPointDefinition
{
    public void RegisterEndPoint(WebApplication webApplication)
    {
        webApplication.MapPost("/pricelists/upload", async (HttpRequest request, IPriceListRepository repo, CancellationToken ct) =>
        {
            using var reader = new StreamReader(request.Body);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            });

            var records = csv.GetRecords<PriceListDto>().ToList();

            var errors = new List<string>();
            var validRecords = new List<PriceListDto>();

            foreach (var record in records)
            {
                if (record.ValidFrom > record.ValidTo)
                {
                    errors.Add($"Invalid date range for SKU {record.Sku}, Supplier {record.SupplierId}");
                    continue;
                }
               
                var existing = await repo.GetAllAsync(ct);
                var overlap = existing.Any(p =>
                    p.SupplierId == record.SupplierId &&
                    p.Sku == record.Sku &&
                    record.ValidFrom <= p.ValidTo &&
                    record.ValidTo >= p.ValidFrom);

                if (overlap)
                {
                    errors.Add($"Overlapping date range for SKU {record.Sku}, Supplier {record.SupplierId}");
                    continue;
                }

                validRecords.Add(record);
            }

            if (validRecords.Any())
            {
                await repo.InsertPriceListCSVAsync(validRecords, ct);
            }

            return Results.Ok(new
            {
                Inserted = validRecords.Count,
                Errors = errors
            });
        });

        webApplication.MapGet("/pricing/best", async (string sku, int qty, string currency, DateTime date, IPriceCalculator calculator,CancellationToken ct) =>
        {
            var result = await calculator.GetBestPriceAsync(sku, qty, currency, date, ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        webApplication.MapGet("/prices", async ( IPriceListRepository repo, CancellationToken ct, string? sku, DateTime? validOn, string? currency, int? supplierId, int page = 1, int pageSize = 10) =>
        {
            var all = await repo.GetAllAsync(ct);

            var filtered = all.AsQueryable();

            if (!string.IsNullOrEmpty(sku))
                filtered = filtered.Where(p => p.Sku == sku);

            if (validOn.HasValue)
                filtered = filtered.Where(p => p.ValidFrom <= validOn && p.ValidTo >= validOn);

            if (!string.IsNullOrEmpty(currency))
                filtered = filtered.Where(p => p.Currency == currency);

            if (supplierId.HasValue)
                filtered = filtered.Where(p => p.SupplierId == supplierId.Value);

            var totalCount = filtered.Count();
            var data = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Results.Ok(new
            {
                Total = totalCount,
                Page = page,
                PageSize = pageSize,
                Data = data
            });
        });

    }
}

