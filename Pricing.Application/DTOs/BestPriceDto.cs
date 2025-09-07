using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Application.DTOs;

public class BestPriceDto
{
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Total { get; set; }
    public string? Reasoning { get; set; }

}
