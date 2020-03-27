using System.Threading.Tasks;
using EasyRates.RatesProvider.OpenExchange;
using FluentAssertions;
using Xunit;

namespace EasyRates.Writer.Tests.WriterOpenExchange
{
    public class OpenExchangeProxyTests
    {
        private OpenExchangeProxy proxy;
        
        public OpenExchangeProxyTests()
        {
            proxy = new OpenExchangeProxy("ae0227f947ee4c6f9e474560377455df");
        }
        
        [Fact]
        public async Task GetsCurrentRates_Success()
        {
            var response = await proxy.GetCurrentRates("usd");

            response.Base.Should().Be("USD");
            response.TimeStamp.Should().NotBe(0);
            response.Rates.Should().NotBeNull();
            response.Rates.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task GetsCurrentRates_Fail()
        {
            await Assert.ThrowsAsync<ErrorOnResponseToOpenExchangeException>(
                async () => await proxy.GetCurrentRates("dich"));
        }
        
        [Fact]
        public async Task GetUsage_Success()
        {
            await proxy.GetUsage();
        }
    }
}