using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodolistBenShared.Clients;
using TodolistBenShared.Interfaces;

namespace TodolistBenAzureWeb
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
            services.AddApplicationInsightsTelemetry();

            services.AddControllers();

            services.AddSignalR().AddAzureSignalR(Configuration.GetValue<string>("ConnectionStrings:AzureSignalRConnectionString"));

            services.AddSingleton<ITodoQueueClient>(s =>
               new TodoQueueClient(
                   Configuration.GetValue<string>("ConnectionStrings:sbConnectionString"),
                   Configuration.GetValue<string>("ConnectionStrings:queueName")));

            services.AddSingleton<ITodoStorageClient>(s =>
               new TodoStorageClient(
                   Configuration.GetValue<string>("ConnectionStrings:storConnectionString"),
                   Configuration.GetValue<string>("ConnectionStrings:containerName")));

            services.AddSingleton<ITodoDbClient>(s =>
               new TodoDbClient(
                   Configuration.GetValue<string>("ConnectionStrings:sqldb_connection")));

            services.AddSingleton<ITodoSearchClient>(s =>
               new TodoSearchClient(
                    Configuration.GetValue<string>("ConnectionStrings:SearchServiceName"),
                   Configuration.GetValue<string>("ConnectionStrings:SearchServiceAdminApiKey"),
                   Configuration.GetValue<string>("ConnectionStrings:SearchIndexName")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TodoHub>("/todo");
            });
        }
    }
}
