using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InjazAcc.UI.ViewModels;
using InjazAcc.UI;

namespace InjazAcc.UI.Views.Pages;

public partial class InvoicesPage : Page
{
    public InvoicesPage()
    {
        InitializeComponent();
        DataContext = App.Get<IServiceProvider>().GetRequiredService<InvoicesViewModel>();
    }
}
