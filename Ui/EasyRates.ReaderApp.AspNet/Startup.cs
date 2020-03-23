using System;
using System.IO;
using AppVersion;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EasyRates.ReaderApp.AspNet
{
    /// <summary>
    /// Configuration class
    /// </summary>
    public class Startup
    {
        private readonly Di.AppConfig config;
        
        private readonly Di.DiModuleProvider moduleProvider;

        private IContainer applicationContainer;
        
        private IAppVersion appVersion;

        /// <summary>
        /// Construct <see cref="Startup"/> instance
        /// </summary>
        /// <param name="config"></param>
        /// <param name="moduleProvider"></param>
        public Startup(
            Di.AppConfig config, 
            Di.DiModuleProvider moduleProvider)
        {
            this.config = config;
            this.moduleProvider = moduleProvider;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // its temp instance
            var serviceProvider = MakeAutofacServiceProvider(services);
            appVersion = serviceProvider.GetService<IAppVersion>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    //options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffzzzz";
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            ConfigureSwagger(services);
            
            return MakeAutofacServiceProvider(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="appLifetime"></param>
        /// <param name="env"></param>
        public void Configure(
            IApplicationBuilder app,
            IApplicationLifetime appLifetime,
            IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Authentication must be before Mvc, otherwise JWT doesn't work.
            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{appVersion.OnlyMajorMinor}/swagger.json", $"{config.ApplicationName} API");
            });
            
            // So that Singleton-services will be disposed.
            appLifetime.ApplicationStopped.Register(() => applicationContainer.Dispose());
        }
        
        #region Private
        
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(appVersion.OnlyMajorMinor, new Info
                {
                    Title = $"{config.ApplicationName} API", 
                    Version = appVersion.OnlyMajorMinor
                });
                
                //Set the comments path for the swagger json and ui.
                foreach (var docFile in Directory.GetFiles(config.BinFolder, "*.xml"))
                {
                    c.IncludeXmlComments(docFile, true);
                }
                c.DescribeAllEnumsAsStrings();
                // Generic types have [] in swagger. Clear them.
                // Generic ErrorMessages have postfix ErrorCode. Clear it.
                c.CustomSchemaIds(type => type.FriendlyId().Replace("[", "").Replace("]", "").Replace("ErrorCode", ""));
            });
        }
        
  
        private IServiceProvider MakeAutofacServiceProvider(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule(moduleProvider.GetModule(config));
            
			applicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(applicationContainer);
        }
        
        #endregion
    }
}