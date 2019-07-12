using Merchant.Core;
using Merchant.Data.Repositories;
using Merchant.Domain.Commands;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace MerchantProgram
{
    public partial class Program
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Repository>();
            services.AddTransient<LineProcessor>();
            services.AddTransient<Calculate>();
        }

        public static void Run()
        {
            var processor = GetService<LineProcessor>();

            while (true)
            {
                Log.Information("Enter your words: ");
                var sentence = Console.ReadLine();

                if(sentence.Equals("EXIT"))
                {
                    break;
                }

                CommandResponse response = processor.ParseAndCalculate(sentence);
                Log.Information("Sucess: {0} | Message: {1}", response.Success, response.Information);
            }
        }
    }
}
