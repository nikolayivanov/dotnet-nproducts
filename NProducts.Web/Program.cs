using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NProducts.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            var host = CreateWebHostBuilder(args).Build();

            var logger = (ILogger<Program>)host.Services.GetService(typeof(ILogger<Program>));
            logger.LogInformation("Application started.");

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    // Requires `using Microsoft.Extensions.Logging;`
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddFile("SeriLogs/NProducts-{Date}.txt");
                });

            return webHost;
        }
    }
}
