using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using InjazAcc.Infrastructure;
using InjazAcc.Services;

namespace InjazAcc.UI;

public partial class App : Application
{
    public static IHost? HostInstance;

    protected override async void OnStartup(StartupEventArgs e)
    {
        HostInstance = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices((ctx, services) =>
            {
                services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=injazacc.db"));
                services.AddScoped<AuthService>();
                services.AddScoped<PostingService>();
                services.AddViewModels();
                services.AddSingleton<Views.LoginWindow>();
            })
            .Build();

        using (var scope = HostInstance.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await DbInitializer.EnsureSeedAsync(db);
        }

        // Show login
        var login = Get<Views.LoginWindow>();
        login.Show();

        base.OnStartup(e);
    }

    public static T Get<T>() where T : notnull => HostInstance!.Services.GetRequiredService<T>();
}
