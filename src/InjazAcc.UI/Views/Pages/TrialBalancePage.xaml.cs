using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InjazAcc.UI.ViewModels;
using InjazAcc.UI;

namespace InjazAcc.UI.Views.Pages;

public partial class TrialBalancePage : Page
{
    public TrialBalancePage()
    {
        InitializeComponent();
        DataContext = App.Get<IServiceProvider>().GetRequiredService<TrialBalanceViewModel>();
    }
}
