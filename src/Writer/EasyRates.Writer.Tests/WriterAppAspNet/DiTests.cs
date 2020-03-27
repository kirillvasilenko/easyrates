using System.Collections.Generic;
using System.Linq;
using EasyRates.WriterApp;
using EasyRates.WriterApp.AspNet;
using EasyRates.WriterApp.AspNet.HostedServices;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace EasyRates.Writer.Tests.WriterAppAspNet
{
    public class DiTests
    {
        private readonly ServiceProvider provider;
        
        public DiTests()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ConnectionStrings:DefaultConnection", "test"}
                })
                .Build();
            
            var services = new ServiceCollection()
                .AddEasyRatesWriterAppAspNet(config)
                .AddLogging();
            
            provider = services.BuildServiceProvider(new ServiceProviderOptions()
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });
        }

        [Fact]
        public void CanCreateAllTheServices()
        {
            var orderedRatesProviders = provider.GetService<IOrderedRatesProviders>();
            orderedRatesProviders.Should().NotBeNull();

            var ratesUpdater = provider.GetService<IRatesUpdater>();
            ratesUpdater.Should().NotBeNull();

            var writerApp = provider.GetService<IWriterApp>();
            writerApp.Should().NotBeNull();

            var hostedServices = provider.GetServices<IHostedService>().ToList();
            hostedServices.Should().NotBeEmpty();
            hostedServices.Should().Contain(x => x is WriterHostedService);

        }
    }
}