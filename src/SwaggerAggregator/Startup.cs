using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwaggerAggregator.HttpService;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAggregator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<SwaggerAggregatorOptions>(Configuration);

            services.AddHttpClient<SwaggerHttpClient>();
            services.AddTransient<ISwaggerProvider, SwaggerAggregatorProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            var swaggerOptions = Configuration.Get<SwaggerAggregatorOptions>();

            app.UseSwagger(options =>
            {   
                options.RouteTemplate = "documentation/{documentName}/swagger.yaml";
            });
            app.UseSwaggerUI(options =>
            {
                var versions = swaggerOptions.Services.GroupBy(x => x.Version);

                foreach (var item in versions)
                {
                    options.SwaggerEndpoint($"/documentation/{item.Key}/swagger.yaml", item.Key);
                    options.RoutePrefix = "documentation";
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
