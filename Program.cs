using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NetPlatformCheckers;
using InfrastructureCheckers;

namespace mvccoresb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RepoAndUOWCheck.GO();
            Check.GO();
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //http host for Fidler http test
            .UseUrls("http://localhost:5000")
                .UseStartup<Startup>();
    }
    
}
