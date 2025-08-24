using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using InjazAcc.Infrastructure;
using InjazAcc.Services;
using InjazAcc.UI.ViewModels;

namespace InjazAcc.UI
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs e)
        {
            SetupGlobalExceptionHandlers();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=injazacc.db"),
                        contextLifetime: ServiceLifetime.Singleton,
                        optionsLifetime: ServiceLifetime.Singleton);

                    // Services
                    services.AddSingleton<PostingService>();
                    services.AddSingleton<ZatcaService>();
                    services.AddSingleton<VatService>();

                    // ViewModels
                    services.AddSingleton<LoginViewModel>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<InvoicesViewModel>();
                    services.AddSingleton<TrialBalanceViewModel>();
                    services.AddSingleton<ReceiptsViewModel>();
                    services.AddSingleton<ExpensesViewModel>();
                })
                .Build();

            Services = host.Services;

            // Seed DB
            var db = Services.GetRequiredService<AppDbContext>();
            DbInitializer.Seed(db);

            // نافذة رئيسية + DataContext
            var mw = new Views.MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };
            mw.Show();

            base.OnStartup(e);
        }

        public static T Get<T>() => (T)Services.GetService(typeof(T))!;

        private static void SetupGlobalExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
                LogAndAlert("UnhandledException", args.ExceptionObject as Exception);

            Current.DispatcherUnhandledException += (_, args) =>
            {
                LogAndAlert("DispatcherUnhandledException", args.Exception);
                args.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (_, args) =>
            {
                LogAndAlert("UnobservedTaskException", args.Exception);
                args.SetObserved();
            };
        }

        private static void LogAndAlert(string kind, Exception? ex)
        {
            try
            {
                var msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{kind}] {ex?.GetType().Name}: {ex?.Message}{Environment.NewLine}{ex?.StackTrace}{Environment.NewLine}----------------{Environment.NewLine}";
                File.AppendAllText("error.log", msg);
            }
            catch { /* ignore */ }

            MessageBox.Show(ex?.Message ?? kind, "حدث خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
