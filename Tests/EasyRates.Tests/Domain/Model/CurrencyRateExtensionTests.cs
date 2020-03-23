using System;
using AutoFixture;
using EasyRates.Model;
using FluentAssertions;
using Xunit;

namespace EasyRates.Tests.Domain.Model
{
    public class CurrencyRateExtensionTests
    {
        private Fixture fixture = new Fixture();
        
        [Fact]
        public void UpdateRateFromAnotherOne()
        {
            var r1 = fixture.Create<CurrencyRate>();
            var r2 = fixture.Create<CurrencyRate>();
            
            r1.Update(r2);
            
            r1.Should().BeEquivalentTo(r2);
        }

        [Fact]
        public void MakesRateHistoryItemFromRate()
        {
            var r1 = fixture.Create<CurrencyRate>();

            var hi = r1.ToHistoryItem();
            
            hi.Should().BeEquivalentTo(r1, c =>
            {
                return c.Excluding(d => d.Key);
            });
            
            r1.Should().BeEquivalentTo(hi, c =>
            {
                return c.Excluding(d => d.Id);
            });
        }
    }
}