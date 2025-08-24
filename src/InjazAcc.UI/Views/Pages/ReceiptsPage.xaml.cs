using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace InjazAcc.UI.Views.Pages
{
    public partial class ReceiptsPage : Page
    {
        public ReceiptsPage()
        {
            InitializeComponent();
            DataContext = UI.App.Get<IServiceProvider>().GetRequiredService<InjazAcc.UI.ViewModels.ReceiptsViewModel>();
        }
    }
}
