namespace Pricing.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string? Sku { get; set; }
    public string? Name { get; set; } 
    public string? Uom { get; set; }
    public string? HazardClass { get; set; } 
}
