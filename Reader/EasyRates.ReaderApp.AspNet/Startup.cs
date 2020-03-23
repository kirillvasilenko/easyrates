using System;
using EasyRates.Model;
using EasyRates.Reader.Ef.Pg;
using EasyRates.Reader.Model;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.ReaderApp.AspNet
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

            services.AddEasyRatesReaderApp();
            services.AddEasyRatesReaderEfPg(Config.GetConnectionString("DefaultConnection"));
            
            services.AddProblemDetails(opts =>
            {
                opts.Map<InvalidCurrencyNameException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));
                opts.Map<NoOneRateFoundException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));
                opts.Map<RateNotFoundException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status404NotFound));
                
                opts.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
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
                /*config.OperationProcessors.Add(new OperationProcessor(ctx =>
                {
                    ctx.OperationDescription.Operation.OperationId = ctx.MethodInfo.Name;
                    return true;
                }));*/
            });
        }
    }
}