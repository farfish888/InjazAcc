using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InjazAcc.Infrastructure;
using System.Collections.ObjectModel;

namespace InjazAcc.UI.ViewModels;

public partial class TrialBalanceViewModel : BaseViewModel
{
    private readonly AppDbContext _db;
    public ObservableCollection<TrialBalanceRow> Rows { get; } = new();
    [ObservableProperty] private DateTime from = new(DateTime.Now.Year, 1, 1);
    [ObservableProperty] private DateTime to = DateTime.Now;

    public TrialBalanceViewModel(AppDbContext db) => _db = db;

    [RelayCommand]
    private async Task Refresh()
    {
        Rows.Clear();
        var data = await ReportQueries.GetTrialBalanceAsync(_db, From, To);
        foreach (var r in data) Rows.Add(r);
    }
}
