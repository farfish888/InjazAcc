using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InjazAcc.Services;
using System.Threading.Tasks;
using System.Windows;

namespace InjazAcc.UI.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly AuthService _auth;

    [ObservableProperty] private string username = "admin";
    [ObservableProperty] private string password = "1234";

    public LoginViewModel(AuthService auth) => _auth = auth;

    [RelayCommand]
    private async Task Login()
    {
        if (await _auth.ValidateAsync(Username, Password))
        {
            // Open dashboard
            var dash = new Views.MainWindow();
            dash.Show();
            foreach (Window w in Application.Current.Windows)
                if (w is Views.LoginWindow) { w.Close(); break; }
        }
        else
        {
            MessageBox.Show("بيانات الدخول غير صحيحة", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
