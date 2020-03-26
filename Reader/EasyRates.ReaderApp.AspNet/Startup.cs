﻿using System;
using EasyRates.Model;
using EasyRates.Model.Ef.Pg;
using EasyRates.Reader.Ef.Pg;
using EasyRates.Reader.Model;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;

namespace EasyRates.ReaderApp.AspNet
{
    public class Startup
    {
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
            services.AddSingleton<ISystemClock, SystemClock>();

            services.AddHealthChecks()
                .AddDbContextCheck<RatesContext>();
            
            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;

                    // automatically applies an api version based on the name of the defining controller's namespace
                    options.Conventions.Add( new VersionByNamespaceConvention() );
                    options.ErrorResponses = new ProblemDetailsErrorResponseProvider();
                } );
            
            services.AddVersionedApiExplorer( options =>
            {
                options.GroupNameFormat = "VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            
            services.AddProblemDetails(opts =>
            {
                opts.Map<InvalidCurrencyNameException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));
                opts.Map<NoOneRateFoundException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));
                opts.Map<RateNotFoundException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status404NotFound));
                
                opts.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services
                .AddOpenApiDocument(config =>
                {
                    config.DocumentName = "v1";
                    config.ApiGroupNames = new[] {"1"};
                    config.PostProcess = doc =>
                    {
                        doc.Info.Version = "v1";
                        doc.Info.Title = $"{Program.AppName} API";
                    };
                })
                .AddOpenApiDocument(config =>
                {
                    config.DocumentName = "v2";
                    config.ApiGroupNames = new[] {"2"};
                    config.PostProcess = doc =>
                    {
                        doc.Info.Version = "v2";
                        doc.Info.Title = $"{Program.AppName} API";
                    };
                });

            /*services.AddOpenApiDocument(config =>
            {
                
                /*config.OperationProcessors.Add(new OperationProcessor(ctx =>
                {
                    ctx.OperationDescription.Operation.OperationId = ctx.MethodInfo.Name;
                    return true;
                }));#1#
            });*/
        }
    }
}