using Pricing.Application.Interfaces;

namespace Pricing.Infrastructure.Repositories;

public class RateProvider : IRateProvider
{
    private readonly Dictionary<string, decimal> _rates = new()
    {
        { "USD:EUR", 0.92m },
        { "EUR:USD", 1.09m },
        { "USD:USD", 1m },
        { "EUR:EUR", 1m }
    };

    public decimal Convert(string fromCurrency, string toCurrency, decimal amount)
    {
        var key = $"{fromCurrency}:{toCurrency}";
        if (_rates.TryGetValue(key, out var rate))
            return amount * rate;

        throw new ArgumentException($"Rate from {fromCurrency} to {toCurrency} not defined.");
    }
}
