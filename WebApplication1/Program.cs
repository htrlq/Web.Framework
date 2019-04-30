using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var index = Calc(110, 20 * 3, 20);
            CreateWebHostBuilder(args).Build().Run();
        }

        private static int Calc(int maxMinute, int afterMinute, int onceMinute)
        {
            return (maxMinute - afterMinute) / onceMinute;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
