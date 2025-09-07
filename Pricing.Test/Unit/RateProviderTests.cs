using Pricing.Infrastructure.Repositories;

namespace Pricing.Test.Unit;

public class RateProviderTests
{
    private readonly RateProvider _rateProvider = new RateProvider();

    [Theory]
    [InlineData("USD", "EUR", 10, 9.2)]
    [InlineData("EUR", "USD", 10, 10.9)]
    [InlineData("USD", "USD", 10, 10)]
    [InlineData("EUR", "EUR", 15, 15)]
    public void Convert_ReturnsCorrectConvertedAmount(string from, string to, decimal amount, decimal expected)
    {
        var result = _rateProvider.Convert(from, to, amount);
        Assert.Equal(expected, result);
    }
}
