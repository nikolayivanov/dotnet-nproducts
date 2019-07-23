using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace NProducts.Tests
{
    internal static class TestHelper
    {
        public static IConfiguration GetIConfiguration()
        {
            return new ConfigurationBuilder()                
                .AddJsonFile("appsettings.json", optional: true)                
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
