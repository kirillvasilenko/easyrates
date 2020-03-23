using AutoFixture;
using EasyRates.Model;
using FluentAssertions;
using Xunit;

namespace EasyRates.Tests.Domain.Model
{
    public class CurrencyRateTests
    {
        private Fixture fixture = new Fixture();
        
        [Fact]
        public void KeyShouldBeConcatFromAndTo()
        {
            var rate = new CurrencyRate()
            {
                From = fixture.Create<string>(),
                To = fixture.Create<string>()
            };

            rate.Key.Should().Be(rate.From + rate.To);
        }
    }
}