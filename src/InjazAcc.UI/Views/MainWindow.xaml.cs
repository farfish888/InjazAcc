using System.Windows;
using System.Windows.Controls;

namespace InjazAcc.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, __) => MainFrame.Navigate(new Pages.InvoicesPage());
    }

    private void Invoices_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.InvoicesPage());
    private void TrialBalance_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.TrialBalancePage());
}
    private void Receipts_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.ReceiptsPage());
