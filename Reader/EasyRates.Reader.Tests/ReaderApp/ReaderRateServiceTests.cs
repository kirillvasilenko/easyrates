using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.Model;
using EasyRates.Reader.Model;
using EasyRates.ReaderApp;
using EasyRates.ReaderApp.Dto;
using FluentAssertions;
using Moq;
using Xunit;

namespace EasyRates.Reader.Tests.ReaderApp
{
    public class ReaderRateServiceTests
    {
        private Fixture fixture = new Fixture();

        private ReaderRateService svc;
        
        private Mock<IRatesReader> ratesReader = new Mock<IRatesReader>(MockBehavior.Strict); 
        
        private Mock<ICurrencyNameFormatter> formatter = new Mock<ICurrencyNameFormatter>(MockBehavior.Strict);
        
        private Mock<ICurrencyNameValidator> validator = new Mock<ICurrencyNameValidator>(MockBehavior.Strict);
        
        public ReaderRateServiceTests()
        {
            svc = new ReaderRateService(ratesReader.Object, formatter.Object, validator.Object );
        }

        [Fact]
        public async Task GetRate_FormatAndValidateArgs_AndConvertResult()
        {
            var rate = fixture.Create<CurrencyRate>();
            var formattedFrom = fixture.Create<string>();
            var formattedTo = fixture.Create<string>();

            formatter.Setup(f => f.FormatName(rate.From)).Returns(formattedFrom);
            formatter.Setup(f => f.FormatName(rate.To)).Returns(formattedTo);

            validator.Setup(v => v.Validate(formattedFrom));
            validator.Setup(v => v.Validate(formattedTo));

            ratesReader.Setup(r => r.GetRate(formattedFrom, formattedTo)).ReturnsAsync(rate);

            var result = await svc.GetRate(rate.From, rate.To);

            var expectedResult = rate.ToDto();
            result.Should().BeEquivalentTo(expectedResult);

        }
        
        [Fact]
        public async Task GetRates_FormatAndValidateArgs_AndConvertResult()
        {
            var rates = fixture.CreateMany<CurrencyRate>().ToList();
            var formattedFrom = fixture.Create<string>();

            formatter.Setup(f => f.FormatName(rates.First().From)).Returns(formattedFrom);

            validator.Setup(v => v.Validate(formattedFrom));
            
            ratesReader.Setup(r => r.GetRates(formattedFrom)).ReturnsAsync(rates);

            var result = await svc.GetRates(rates.First().From);

            var expectedResult = rates.ToDto();
            result.Should().BeEquivalentTo(expectedResult);
        } 
    }
}