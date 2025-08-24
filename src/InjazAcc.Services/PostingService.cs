using InjazAcc.Domain;
using InjazAcc.Infrastructure;
using InjazAcc.Shared;
using Microsoft.EntityFrameworkCore;

namespace InjazAcc.Services;

public class PostingService(AppDbContext db)
{
    // Map codes for demo
    private const string ACC_AR = "1100";   // Accounts Receivable
    private const string ACC_REV = "4100";  // Revenue
    private const string ACC_VAT_OUT = "2130"; // VAT Output

    private async Task<int> AccountIdByCode(string code)
        => (await db.Accounts.FirstAsync(a => a.Code == code)).Id;

    public async Task<Result> PostInvoiceAsync(int invoiceId, string createdBy = "system")
    {
        var inv = await db.Invoices.Include(i => i.Lines).FirstOrDefaultAsync(i => i.Id == invoiceId);
        if (inv is null) return Result.Fail("Invoice not found");

        var ar = await AccountIdByCode(ACC_AR);
        var rev = await AccountIdByCode(ACC_REV);
        var vatOut = await AccountIdByCode(ACC_VAT_OUT);

        // Build entry
        var entry = new JournalEntry
        {
            Date = inv.Date,
            Ref = inv.Number,
            DocType = DocType.Invoice,
            DocId = inv.Id,
            BranchId = inv.BranchId,
            CostCenterId = inv.CostCenterId,
            CreatedBy = createdBy
        };

        // Totals assumed pre-calculated on invoice
        entry.Lines.Add(new JournalLine { AccountId = ar, Debit = inv.TotalInclVat, Credit = 0, Note = "A/R" });
        entry.Lines.Add(new JournalLine { AccountId = rev, Debit = 0, Credit = inv.TotalExclVat, Note = "Revenue" });
        if (inv.VatAmount != 0)
            entry.Lines.Add(new JournalLine { AccountId = vatOut, Debit = 0, Credit = inv.VatAmount, Note = "VAT Output" });

        if (Math.Round(entry.Lines.Sum(l => l.Debit),2) != Math.Round(entry.Lines.Sum(l => l.Credit),2))
            return Result.Fail("Entry not balanced");

        db.JournalEntries.Add(entry);
        await db.SaveChangesAsync();
        return Result.Ok($"Posted JE #{entry.Id}");
    }
}

