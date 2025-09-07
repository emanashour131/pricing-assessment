namespace Pricing.Domain.Entities;

public class PriceList
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string? Sku { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string? Currency { get; set; } 
    public decimal PricePerUom { get; set; }
    public int MinQty { get; set; }
}
