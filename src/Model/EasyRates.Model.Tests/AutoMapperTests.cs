using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyRates.Model.Tests
{
    public class AutoMapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var provider = new ServiceCollection()
                .AddEasyRatesModel()
                .BuildServiceProvider();
            
            var mapper = provider.GetService<IMapper>();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}