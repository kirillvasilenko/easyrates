using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Amursoft.AspNetCore.TestAuthentication;
using EasyRates.Model;
using EasyRates.Model.Ef.Pg;
using EasyRates.Reader.Ef.Pg;
using EasyRates.Reader.Model;
using EasyRates.Reader.Spanner;
using Hellang.Middleware.ProblemDetails;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.IdentityModel.Tokens;

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
            ConfigureAuthentication(services);
            ConfigureAuthorization3(services);
            
            ConfigureSwagger(services);

            services.AddEasyRatesReaderApp();
            if (Config.GetValue<string>("DbType") == "Spanner")
            {
                services.AddEasyRatesReaderEfSpanner(Config.GetConnectionString("Spanner"));
            }
            else
            {
                services.AddEasyRatesReaderEfPg(Config.GetConnectionString("DefaultConnection"));    
            }
            
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
            
            app.UseAuthentication();
            app.UseAuthorization();

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
        }
        
        private void ConfigureAuthentication(IServiceCollection services)
        {
            if (Config.GetSection("Auth").GetValue("UseTestAuth", false))
            {
                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = TestAuth.SchemeName;
                        options.DefaultChallengeScheme = TestAuth.SchemeName;
                    })
                    .AddTestAuth(opts =>
                    {
                        opts.Identity.AddClaim(new Claim(JwtClaimTypes.Scope, "easyrate.reader.read"));
                        opts.Identity.AddClaim(new Claim(JwtClaimTypes.Scope, "easyrate.reader.admin"));
                    });
                return;
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Config["Auth:Issuer"],
                        
                    ValidAudience = Config["Auth:Audience"],
                        
                    ClockSkew = TimeSpan.FromSeconds(0),
 
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(
                            Config[$"Auth:Secret"])),
                                
                    NameClaimType = JwtClaimTypes.Subject,
                    RoleClaimType = JwtClaimTypes.Role,
                };
                var tokenValidator = options.SecurityTokenValidators.OfType<JwtSecurityTokenHandler>().First();
                tokenValidator.MapInboundClaims = false;
            });
        }

        private void ConfigureAuthorization3(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim(JwtClaimTypes.Role, "Admin");
                    policy.RequireClaim(JwtClaimTypes.Scope, "easyrate.reader.admin");
                });
                options.AddPolicy("Client", policy =>
                {
                    policy.RequireClaim(JwtClaimTypes.Role, "Client", "Admin");
                    policy.RequireClaim(JwtClaimTypes.Scope, "easyrate.reader.read");
                });
            });
        }
    }
}