using AutoFixture;
using FluentAssertions;
using Xunit;

namespace EasyRates.Model.Tests
{
    public class CurrencyNameFormatterTests
    {
        private readonly Fixture fixture = new Fixture();
        
        private readonly CurrencyNameFormatter formatter = new CurrencyNameFormatter();
        
        [Fact]
        public void FormatsToUpperCase()
        {
            var name = fixture.Create<string>();

            var formatted = formatter.FormatName(name);

            formatted.Should().Be(name.ToUpper());
        }
    }
}