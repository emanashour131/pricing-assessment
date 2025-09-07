using Microsoft.AspNetCore.Mvc;
using Pricing.Application.DTOs;
using Pricing.Application.Interfaces;

namespace Pricing.Api.EndPointDefinitions;

public class SupplierEndPointDefinition : IEndPointDefinition
{
    public void RegisterEndPoint(WebApplication webApplication)
    {
        webApplication.MapPost("api/Suppliers", async ([FromBody] SupplierDto supplierDto, ISupplierRepository supplierRepository, CancellationToken ct) =>
        {
            return await supplierRepository.CreateAsync(supplierDto,ct);
        });

        webApplication.MapGet("api/Suppliers", async (ISupplierRepository supplierRepository, CancellationToken ct) =>
        {
            return await supplierRepository.GetAllAsync(ct);
        });

        webApplication.MapGet("api/Suppliers/{id}", async ([FromRoute] int id, ISupplierRepository supplierRepository, CancellationToken ct) =>
        {
            return await supplierRepository.GetByIdAsync(id, ct);
        });

        webApplication.MapPut("api/Suppliers/{id}", async ([FromBody] SupplierDto supplierDto, [FromRoute] int id, ISupplierRepository supplierRepository, CancellationToken ct) =>
        {
            return await supplierRepository.UpdateAsync(id, supplierDto,ct);
        });

        webApplication.MapDelete("api/Suppliers/{id}", async ([FromRoute] int id, ISupplierRepository supplierRepository, CancellationToken ct) =>
        {
            await supplierRepository.DeleteAsync(id,ct);
        });
    }
}

