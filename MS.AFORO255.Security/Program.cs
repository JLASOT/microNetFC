using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace MS.AFORO255.Security
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((host, builder) =>
                    {
                        var env = host.HostingEnvironment;
                        builder.AddConfigServer(env.EnvironmentName);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
