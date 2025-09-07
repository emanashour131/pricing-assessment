using Microsoft.AspNetCore.Mvc;
using Pricing.Application.DTOs;
using Pricing.Application.Interfaces;

namespace Pricing.Api.EndPointDefinitions;

public class ProductEndPointDefinition : IEndPointDefinition
{
    public void RegisterEndPoint(WebApplication webApplication)
    {
        webApplication.MapPost("api/Products", async ([FromBody] ProductDto productDto, IProductRepository productRepository, CancellationToken ct) =>
        {
            return await productRepository.CreateAsync(productDto,ct);
        });

        webApplication.MapGet("api/Products", async (IProductRepository productRepository, CancellationToken ct) =>
        {
            return await productRepository.GetAllAsync(ct);
        });

        webApplication.MapGet("api/Products/{id}", async ([FromRoute] int id, IProductRepository productRepository, CancellationToken ct) =>
        {
            return await productRepository.GetByIdAsync(id, ct);
        });

        webApplication.MapPut("api/Products/{id}", async ([FromBody] ProductDto productDto, [FromRoute] int id, IProductRepository productRepository, CancellationToken ct) =>
        {
            return await productRepository.UpdateAsync(id, productDto,ct);
        });

        webApplication.MapDelete("api/Products/{id}", async ([FromRoute] int id, IProductRepository productRepository, CancellationToken ct) =>
        {
            await productRepository.DeleteAsync(id,ct);
        });
    }
}

