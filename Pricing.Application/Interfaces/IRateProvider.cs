namespace Pricing.Application.Interfaces;

public interface IRateProvider
{
    decimal Convert(string fromCurrency, string toCurrency, decimal amount);
}
