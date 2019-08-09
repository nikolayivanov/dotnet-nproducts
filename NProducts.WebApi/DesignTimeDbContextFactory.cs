using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NProducts.DAL.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NProducts.WebApi.Context
{
    /// <summary>
    /// If a class implementing this interface is found in either the same project as the derived DbContext or in the application's 
    /// startup project, the tools bypass the other ways of creating the DbContext and use the design-time factory instead.
    /// https://blog.rodrigo-santos.me/mjalnXQC3wI0FvuLzxPF
    /// </summary>    
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NorthwindContext>
    {
        public NorthwindContext CreateDbContext(string[] args)
        {
            var cbuilder = new ConfigurationBuilder();
            
            IConfigurationRoot configuration = ((IConfigurationBuilder)cbuilder).SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../NProducts.WebApi/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<NorthwindContext>();
            var connectionString = configuration.GetConnectionString("NorthwindDB");
            builder.UseSqlServer(connectionString);
            return new NorthwindContext(builder.Options);
        }
    }
}
