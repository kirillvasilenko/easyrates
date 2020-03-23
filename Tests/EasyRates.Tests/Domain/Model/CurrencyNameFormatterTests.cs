using AutoFixture;
using EasyRates.Model;
using FluentAssertions;
using Xunit;

namespace EasyRates.Tests.Domain.Model
{
    public class CurrencyNameFormatterTests
    {
        private Fixture fixture = new Fixture();
        
        private CurrencyNameFormatter formatter = new CurrencyNameFormatter();
        
        [Fact]
        public void FormatsToUpperCase()
        {
            var name = fixture.Create<string>();

            var formatted = formatter.FormatName(name);

            formatted.Should().Be(name.ToUpper());
        }
    }
}