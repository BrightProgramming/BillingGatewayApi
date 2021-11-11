using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Payment.Gateway.Api
{
    /// <summary>
    /// Main bootstrap
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main bootstrap
        /// </summary>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the host builder
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
