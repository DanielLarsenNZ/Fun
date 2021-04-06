//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System;

//namespace MyFun
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .ConfigureServices((hostContext, services) =>
//            {
//                services.AddHostedService<Worker>();
//            });
//    }
//}
