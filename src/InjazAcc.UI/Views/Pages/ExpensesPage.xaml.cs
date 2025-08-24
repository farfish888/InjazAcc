using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace InjazAcc.UI.Views.Pages
{
    public partial class ExpensesPage : Page
    {
        public ExpensesPage()
        {
            InitializeComponent();
            DataContext = UI.App.Get<IServiceProvider>().GetRequiredService<InjazAcc.UI.ViewModels.ExpensesViewModel>();
        }
    }
}
