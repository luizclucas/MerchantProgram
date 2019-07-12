using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
        }
    }
}
