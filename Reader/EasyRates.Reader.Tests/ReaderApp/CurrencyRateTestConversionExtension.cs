using EasyRates.Model;
using EasyRates.ReaderApp.Dto;
using FluentAssertions;

namespace EasyRates.Reader.Tests.ReaderApp
{
    public static class CurrencyRateTestConversionExtension
    {
        public static void CheckEquivalence(this CurrencyRate rate, RateInfo info)
        {
            info.Rate.Should().Be(rate.Value);
            info.To.Should().Be(rate.To);
            info.ExpireAt.Should().Be(rate.ExpirationTime);
        }
    }
}