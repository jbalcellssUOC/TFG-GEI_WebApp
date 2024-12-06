using NLog;
using NLog.Web;
using System;

namespace ExternalAPI
{
    public class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();           // Use startup class    
                    webBuilder.UseUrls("http://0.0.0.0:8155");  // Set the URL for the API
                    webBuilder.UseNLog();                       // NLog: setup NLog for Dependency injection
                });
    }
}
