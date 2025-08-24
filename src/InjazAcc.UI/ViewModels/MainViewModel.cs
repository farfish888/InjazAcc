using CommunityToolkit.Mvvm.ComponentModel;

namespace InjazAcc.UI.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty] private string title = "لوحة التحكم — نظام محاسبي للحراسات";
}
