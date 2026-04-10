using Avalonia;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SongsInLearning.ViewModels;
using System;
using System.Net;
using System.Text;

namespace SongsInLearning
{
    internal class Program
    {
        public static IHost AppHost { get; set; }

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) 
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            AppHost = Host.CreateDefaultBuilder(args)               
               .ConfigureServices((context, services) =>
               {
                   services.AddSingleton(context.Configuration);
                   services.AddTransient<MainViewModel>();

               })
               .Build();
          
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);

        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
