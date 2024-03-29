﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NProducts.DAL;
using NProducts.DAL.Context;
using NProducts.Data.Common;
using NProducts.Data.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace NProducts.WebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My NProducts API", Version = "v1" });
            });

            services.Configure<NProductsOptions>(Configuration.GetSection("NproductsWebOptions"));
            services.AddTransient<IUnitOfWork, NorthwindUnitOfWork>();

            services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NorthwindDB"), b => b.MigrationsAssembly("NProducts.WebApi")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("Application path: {ContentRootPath}", env.ContentRootPath);
                using (logger.BeginScope("Application Configuration:"))
                {
                    foreach (var val in Configuration.AsEnumerable())
                    {
                        logger.LogInformation("{key} - {value}", val.Key, val.Value);
                    }
                }

                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                // To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty string:
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();            
        }
    }
}
