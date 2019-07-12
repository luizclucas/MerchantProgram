using Merchant.Domain;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantProgram
{
    public partial class Program
    {
        public static IServiceProvider RootServiceProvider;

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341", compact: true)
                .CreateLogger();

            try
            {
                var services = new ServiceCollection();
                ConfigureServices(services);
                var sp = services.BuildServiceProvider();
                DIProps.ServiceProvider = RootServiceProvider = sp;
                Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error at Running MerchantProgram");
            }
            finally
            {
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
        }

        public static T GetService<T>() => RootServiceProvider.GetRequiredService<T>();
    }
}
