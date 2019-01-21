using HiveMind.Services.Graph.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neo4jClient;
using System;

namespace HiveMind.Services.Graph
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IGraphService, GraphService>();

            GraphClient graphClient = CreateGraphClient();
            services.AddSingleton<IGraphClient>(graphClient);
        }

        private GraphClient CreateGraphClient()
        {
            var serializer = new CustomNeo4jSerializer();
            var conn = Configuration.GetValue<string>("Neo4j:conn");
            var graphClient = new GraphClient(new Uri(conn))
            {
                UseJsonStreamingIfAvailable = true,
                JsonContractResolver = serializer
            };
            graphClient.Connect();
            return graphClient;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}