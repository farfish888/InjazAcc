using Microsoft.Extensions.DependencyInjection;
using InjazAcc.UI.ViewModels;

namespace InjazAcc.UI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection s)
    {
        s.AddSingleton<LoginViewModel>();
        s.AddSingleton<MainViewModel>();
        s.AddSingleton<InvoicesViewModel>();
        s.AddSingleton<TrialBalanceViewModel>();
                s.AddSingleton<ReceiptsViewModel>();
        return s;
    }
}
