using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace TodolistBenAzureWeb
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            logging.AddDebug();
                        }

                        logging.AddApplicationInsights((string)hostingContext
                            .Configuration
                            .GetValue(typeof(string), "ApplicationInsights:InstrumentationKey"));
                        logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Trace);
                        logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Warning);
                    })
                    .UseStartup<Startup>());
    }
}
