using Avalonia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SongsInLearning.Database;
using SongsInLearning.Services;
using SongsInLearning.ViewModels;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SongsInLearning
{
    internal class Program
    {
        public static IHost AppHost { get; set; }
        private static readonly string LogPath = System.IO.Path.Combine("D:\\SongsInLearning\\Logs", "log.txt");

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) 
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                LogException(e.ExceptionObject as Exception, "AppDomain.UnhandledException");
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                LogException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved(); // Impede que a Task derrube o programa imediatamente
            };

            try
            {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            AppHost = Host.CreateDefaultBuilder(args)               
               .ConfigureServices((context, services) =>
               {
                   services.AddDbContext<MusicDbContext>(options =>
                   {
                       options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"));
                   }, ServiceLifetime.Transient);

                   services.AddSingleton(context.Configuration);
                   services.AddTransient<IAService>();
                   services.AddTransient<SongService>();
                   services.AddSingleton<VstPluginService>();

                   services.AddSingleton<MainViewModel>();

                   services.AddTransient<HomeViewModel>();
                   services.AddTransient<CreateSongViewModel>();
                   services.AddTransient<NotificationViewModel>();

               })
               .Build();
          
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                // Captura qualquer erro fatal durante a inicialização ou injeção de dependência
                LogException(ex, "Main Execution (Fatal)");
                throw; // Lança a exceção novamente para que o SO encerre o processo corretamente
            }

        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();

        private static void LogException(Exception? ex, string source)
        {
            if (ex == null) return;

            try
            {
                // Monta a mensagem formatada com data, hora, origem e o rastro do erro (StackTrace)
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] - Origem do Crash: {source}");
                errorMessage.AppendLine($"Mensagem: {ex.Message}");
                errorMessage.AppendLine($"Stack Trace: {ex.StackTrace}");

                // Se houver uma exceção interna (erro original que causou esse erro), loga também
                if (ex.InnerException != null)
                {
                    errorMessage.AppendLine($"--- Inner Exception ---");
                    errorMessage.AppendLine($"Mensagem: {ex.InnerException.Message}");
                    errorMessage.AppendLine($"Stack Trace: {ex.InnerException.StackTrace}");
                }

                errorMessage.AppendLine(new string('-', 60)); // Separador visual para facilitar a leitura

                // Adiciona o erro ao final do arquivo de log, liberando o arquivo imediatamente após escrever
                File.AppendAllText(LogPath, errorMessage.ToString());
            }
            catch
            {
                // Caso ocorra um erro ao tentar salvar o arquivo de log (ex: falta de permissão na pasta D:),
                // nós engolimos silenciosamente. Se lançássemos um erro aqui, criaríamos um loop infinito de crashes.
            }
        }
    }

   
    }

