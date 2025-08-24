using System.Windows;

namespace InjazAcc.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (_, __) => MainFrame.Navigate(new Pages.InvoicesPage());
        }

        private void Invoices_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.InvoicesPage());
        private void Receipts_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.ReceiptsPage());
        private void Expenses_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.ExpensesPage());
        private void TrialBalance_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.TrialBalancePage());
    }
}
