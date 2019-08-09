using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NProducts.DAL;
using NProducts.DAL.Context;
using NProducts.Data.Common;
using NProducts.Data.Interfaces;
using NProducts.Web.Filters;

namespace NProducts.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<NProductsOptions>(Configuration.GetSection("NproductsWebOptions"));

            // Add service filters.
            services.AddScoped<LogActionAttribute>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IUnitOfWork, NorthwindUnitOfWork>();

            services.AddDbContext<NorthwindContext>(options => 
            options.UseSqlServer(
                Configuration.GetConnectionString("NorthwindDB"), 
                b => b.MigrationsAssembly("NProducts.Web")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("SeriLogs/nproducts.{Date}.txt");
            logger.LogInformation("Application path: {ContentRootPath}", env.ContentRootPath);
            using (logger.BeginScope("Application Configuration:"))
            {
                var strbuilder = new StringBuilder();
                foreach (var val in Configuration.AsEnumerable())
                {
                    strbuilder.AppendLine($"{val.Key} - {val.Value}");
                    logger.LogInformation("{key} - {value}", val.Key, val.Value);
                }

                logger.LogInformation(strbuilder.ToString());
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions() {
                    SourceCodeLineCount = 10
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseStatusCodePages();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
