using System.Windows;
using System.Windows.Controls;
using InjazAcc.UI.ViewModels;
using InjazAcc.UI;
using Microsoft.Extensions.DependencyInjection;

namespace InjazAcc.UI.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _vm;
    public LoginWindow()
    {
        InitializeComponent();
        _vm = App.Get<IServiceProvider>().GetRequiredService<LoginViewModel>();
        DataContext = _vm;
    }

    private void Login_Click(object sender, RoutedEventArgs e) => _vm.GetType().GetMethod("Login", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.Invoke(_vm, new object[] { });
    private void Pwd_PasswordChanged(object sender, RoutedEventArgs e)
    {
        var prop = _vm.GetType().GetProperty("Password", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        prop?.SetValue(_vm, (sender as PasswordBox)?.Password);
    }
}

