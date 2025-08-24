using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InjazAcc.Domain;
using InjazAcc.Infrastructure;
using InjazAcc.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace InjazAcc.UI.ViewModels;

public partial class InvoicesViewModel : BaseViewModel
{
    private readonly AppDbContext _db;
    private readonly PostingService _posting;

    public ObservableCollection<Invoice> Items { get; } = new();

    [ObservableProperty] private string customerName = "عميل افتراضي";
    [ObservableProperty] private decimal amount = 1000m;
    [ObservableProperty] private decimal vat = 0m;
    [ObservableProperty] private decimal total = 0m;
    [ObservableProperty] private string number = "";

    public InvoicesViewModel(AppDbContext db, PostingService posting)
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

    [RelayCommand] private async Task Save()
    {
        var cust = await _db.Customers.FirstOrDefaultAsync(c => c.Name == CustomerName);
        if (cust is null) { cust = new Customer { Name = CustomerName }; _db.Customers.Add(cust); await _db.SaveChangesAsync(); }

        var inv = new Invoice
        {
            Number = string.IsNullOrWhiteSpace(Number) ? $"INV-{DateTime.Now:yyyyMMddHHmmss}" : Number,
            Date = DateTime.Now,
            CustomerId = cust.Id,
            TotalExclVat = Amount,
            VatAmount = Vat,
            TotalInclVat = Total,
            Lines = new List<InvoiceLine> { new() { Description = "خدمة حراسة", Qty = 1, UnitPrice = Amount } }
        };

        // Minimal ZATCA: UUID + QR + UBL placeholder
        inv.Uuid = Guid.NewGuid().ToString();
        inv.QrBase64 = ZatcaService.BuildQrBase64("شركة الحراسات", "3000000000", inv.Date, inv.TotalInclVat, inv.VatAmount);

        _db.Invoices.Add(inv);
        await _db.SaveChangesAsync();
        await _posting.PostInvoiceAsync(inv.Id, "ui");

        Items.Insert(0, inv);
    }

    private async Task Load()
    {
        var list = await _db.Invoices.OrderByDescending(i => i.Id).Take(50).ToListAsync();
        Items.Clear();
        foreach (var i in list) Items.Add(i);
    }
}
