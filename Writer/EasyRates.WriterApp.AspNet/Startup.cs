using System;
using EasyRates.RatesProvider.InMemory;
using EasyRates.RatesProvider.OpenExchange;
using EasyRates.Writer.Ef.Pg;
using EasyRates.WriterApp.AspNet.HostedServices;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EasyRates.WriterApp.AspNet
{
    public class Startup
    {
        private const string Version = "v1";
        
        public Startup(IConfiguration config)
        {
            Config = config;
        }

        public IConfiguration Config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(option =>
            {
                option.LowercaseUrls = true;
            });
            
            ConfigureSwagger(services);

            services.AddEasyRatesWriterAppAspNet(Config);
            
            services.AddProblemDetails(opts =>
            {
                opts.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            app.UseRouting();
            
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
        
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = Version;
                    document.Info.Title = $"{Program.AppName} API";	
                };
            });
        }
    }
}