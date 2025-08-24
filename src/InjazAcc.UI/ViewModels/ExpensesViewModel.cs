using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InjazAcc.Domain;
using InjazAcc.Infrastructure;
using InjazAcc.Services;
using InjazAcc.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace InjazAcc.UI.ViewModels;

public partial class ExpensesViewModel : BaseViewModel
{
    private readonly AppDbContext _db;
    private readonly PostingService _posting;

    public ObservableCollection<ExpenseVoucher> Items { get; } = new();
    public ObservableCollection<Account> ExpenseAccounts { get; } = new();

    [ObservableProperty] private string payee = "مورد";
    [ObservableProperty] private decimal amount = 100m;   // صافي بدون ضريبة
    [ObservableProperty] private decimal vat = 0m;
    [ObservableProperty] private decimal total = 0m;
    [ObservableProperty] private PaymentMethod method = PaymentMethod.Cash;
    public Array Methods => Enum.GetValues(typeof(PaymentMethod));

    [ObservableProperty] private Account? selectedAccount;
    [ObservableProperty] private string number = "";

    public ExpensesViewModel(AppDbContext db, PostingService posting)
    {
        _db = db; _posting = posting;
        _ = Load();
        Recalc();
    }

    [RelayCommand] private void Recalc()
    {
        var (net, v, g) = VatService.Compute(Amount);
        Vat = v; Total = g;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (SelectedAccount is null)
        {
            System.Windows.MessageBox.Show("اختر حساب المصروف", "تنبيه");
            return;
        }

        var exp = new ExpenseVoucher
        {
            Number = string.IsNullOrWhiteSpace(Number) ? $"EXP-{DateTime.Now:yyyyMMddHHmmss}" : Number,
            Date = DateTime.Now,
            Payee = Payee,
            AccountId = SelectedAccount.Id,
            AmountExclVat = Amount,
            VatAmount = Vat,
            TotalAmount = Total,
            Method = Method
        };

        _db.Expenses.Add(exp);
        await _db.SaveChangesAsync();
        await _posting.PostExpenseAsync(exp.Id, "ui");
        Items.Insert(0, exp);
    }

    private async Task Load()
    {
        var list = await _db.Expenses.OrderByDescending(i => i.Id).Take(50).ToListAsync();
        Items.Clear();
        foreach (var i in list) Items.Add(i);

        var exAccs = await _db.Accounts.Where(a => a.Type == AccountType.Expense).OrderBy(a => a.Code).ToListAsync();
        ExpenseAccounts.Clear();
        foreach (var a in exAccs) ExpenseAccounts.Add(a);
        SelectedAccount ??= ExpenseAccounts.FirstOrDefault();
    }
}
