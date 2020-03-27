using System.Linq;
using AutoFixture;
using EasyRates.Model;
using EasyRates.ReaderApp.Dto;
using FluentAssertions;
using Xunit;

namespace EasyRates.Reader.Tests.ReaderApp
{
    public class CurrencyRateToDtoConverterTests
    {
        private Fixture fixture = new Fixture();

        [Fact]
        public void CurrencyRatesToResponse()
        {
            var rates = fixture.CreateMany<CurrencyRate>().ToList();

            var dto = rates.ToDto();

            dto.CurrencyFrom.Should().Be(rates.First().CurrencyFrom);

            for (int i = 0; i < rates.Count; i++)
            {
                rates[i].CheckEquivalence(dto.Rates[i]);
            }
        }

        [Fact]
        public void CurrencyRateToRateInfo()
        {
            var rate = fixture.Create<CurrencyRate>();

            var info = rate.ToRateInfo();

            rate.CheckEquivalence(info);
        }

    }
}