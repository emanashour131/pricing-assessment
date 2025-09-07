namespace Pricing.Application.DTOs;

public class SupplierDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public bool Active { get; set; }
    public bool Preferred { get; set; }
    public int LeadTimeDays { get; set; }
}
