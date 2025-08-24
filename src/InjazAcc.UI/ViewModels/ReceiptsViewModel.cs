using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InjazAcc.Domain;
using InjazAcc.Infrastructure;
using InjazAcc.Services;
using InjazAcc.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace InjazAcc.UI.ViewModels;

public partial class ReceiptsViewModel : BaseViewModel
{
    private readonly AppDbContext _db;
    private readonly PostingService _posting;

    public ObservableCollection<Receipt> Items { get; } = new();

    [ObservableProperty] private string customerName = "عميل افتراضي";
    [ObservableProperty] private decimal amount = 500m;
    [ObservableProperty] private PaymentMethod method = PaymentMethod.Cash;
    public Array Methods => Enum.GetValues(typeof(PaymentMethod));

    [ObservableProperty] private string number = "";

    public ReceiptsViewModel(AppDbContext db, PostingService posting)
    {
        _db = db; _posting = posting;
        _ = Load();
    }

    [RelayCommand]
    private async Task Save()
    {
        var cust = await _db.Customers.FirstOrDefaultAsync(c => c.Name == CustomerName);
        if (cust is null) { cust = new Customer { Name = CustomerName }; _db.Customers.Add(cust); await _db.SaveChangesAsync(); }

        var rcpt = new Receipt
        {
            Number = string.IsNullOrWhiteSpace(Number) ? $"RCPT-{DateTime.Now:yyyyMMddHHmmss}" : Number,
            Date = DateTime.Now,
            CustomerId = cust.Id,
            Amount = Amount,
            Method = Method
        };

        _db.Receipts.Add(rcpt);
        await _db.SaveChangesAsync();
        await _posting.PostReceiptAsync(rcpt.Id, "ui");

        Items.Insert(0, rcpt);
    }

    private async Task Load()
    {
        var list = await _db.Receipts.OrderByDescending(i => i.Id).Take(50).ToListAsync();
        Items.Clear();
        foreach (var i in list) Items.Add(i);
    }
}
