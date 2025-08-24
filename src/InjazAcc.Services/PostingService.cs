using InjazAcc.Domain;
using InjazAcc.Infrastructure;
using InjazAcc.Shared;
using Microsoft.EntityFrameworkCore;

namespace InjazAcc.Services;

public class PostingService(AppDbContext db)
{
    private const string ACC_AR = "1100";      // A/R
    private const string ACC_REV = "4100";     // Revenue
    private const string ACC_VAT_OUT = "2130"; // VAT Output
    private const string ACC_VAT_IN  = "1140"; // VAT Input
    private const string ACC_CASH = "1000";    // Cash
    private const string ACC_BANK = "1010";    // Bank

    private async Task<int> AccountIdByCode(string code)
        => (await db.Accounts.FirstAsync(a => a.Code == code)).Id;

    public async Task<Result> PostInvoiceAsync(int invoiceId, string createdBy = "system")
    {
        var inv = await db.Invoices.Include(i => i.Lines).FirstOrDefaultAsync(i => i.Id == invoiceId);
        if (inv is null) return Result.Fail("Invoice not found");

        var ar = await AccountIdByCode(ACC_AR);
        var rev = await AccountIdByCode(ACC_REV);
        var vatOut = await AccountIdByCode(ACC_VAT_OUT);

        var entry = new JournalEntry
        {
            Date = inv.Date, Ref = inv.Number, DocType = DocType.Invoice, DocId = inv.Id,
            BranchId = inv.BranchId, CostCenterId = inv.CostCenterId, CreatedBy = createdBy
        };

        entry.Lines.Add(new JournalLine { AccountId = ar,  Debit = inv.TotalInclVat, Credit = 0, Note = "A/R" });
        entry.Lines.Add(new JournalLine { AccountId = rev, Debit = 0, Credit = inv.TotalExclVat, Note = "Revenue" });
        if (inv.VatAmount != 0)
            entry.Lines.Add(new JournalLine { AccountId = vatOut, Debit = 0, Credit = inv.VatAmount, Note = "VAT Output" });

        if (Math.Round(entry.Lines.Sum(l => l.Debit),2) != Math.Round(entry.Lines.Sum(l => l.Credit),2))
            return Result.Fail("Entry not balanced");

        db.JournalEntries.Add(entry);
        await db.SaveChangesAsync();
        return Result.Ok($"Posted JE #{entry.Id}");
    }

    public async Task<Result> PostReceiptAsync(int receiptId, string createdBy = "system")
    {
        var rec = await db.Receipts.FirstOrDefaultAsync(r => r.Id == receiptId);
        if (rec is null) return Result.Fail("Receipt not found");

        var ar = await AccountIdByCode(ACC_AR);
        var drAcc = rec.Method == PaymentMethod.Cash ? await AccountIdByCode(ACC_CASH) : await AccountIdByCode(ACC_BANK);

        var entry = new JournalEntry
        {
            Date = rec.Date, Ref = rec.Number, DocType = DocType.Receipt, DocId = rec.Id,
            BranchId = rec.BranchId, CostCenterId = rec.CostCenterId, CreatedBy = createdBy
        };

        entry.Lines.Add(new JournalLine { AccountId = drAcc, Debit = rec.Amount, Credit = 0, Note = rec.Method.ToString() });
        entry.Lines.Add(new JournalLine { AccountId = ar,    Debit = 0, Credit = rec.Amount, Note = "A/R" });

        if (Math.Round(entry.Lines.Sum(l => l.Debit),2) != Math.Round(entry.Lines.Sum(l => l.Credit),2))
            return Result.Fail("Entry not balanced");

        db.JournalEntries.Add(entry);
        await db.SaveChangesAsync();
        return Result.Ok($"Posted receipt JE #{entry.Id}");
    }

    public async Task<Result> PostExpenseAsync(int expenseId, string createdBy = "system")
    {
        var exp = await db.Expenses.FirstOrDefaultAsync(e => e.Id == expenseId);
        if (exp is null) return Result.Fail("Expense not found");

        var vatIn = await AccountIdByCode(ACC_VAT_IN);
        var crAcc = exp.Method == PaymentMethod.Cash ? await AccountIdByCode(ACC_CASH) : await AccountIdByCode(ACC_BANK);

        var entry = new JournalEntry
        {
            Date = exp.Date, Ref = exp.Number, DocType = DocType.Expense, DocId = exp.Id,
            BranchId = exp.BranchId, CostCenterId = exp.CostCenterId, CreatedBy = createdBy
        };

        entry.Lines.Add(new JournalLine { AccountId = exp.AccountId, Debit = exp.AmountExclVat, Credit = 0, Note = "Expense" });
        if (exp.VatAmount != 0)
            entry.Lines.Add(new JournalLine { AccountId = vatIn, Debit = exp.VatAmount, Credit = 0, Note = "VAT Input" });
        entry.Lines.Add(new JournalLine { AccountId = crAcc, Debit = 0, Credit = exp.TotalAmount, Note = exp.Method.ToString() });

        if (Math.Round(entry.Lines.Sum(l => l.Debit),2) != Math.Round(entry.Lines.Sum(l => l.Credit),2))
            return Result.Fail("Entry not balanced");

        db.JournalEntries.Add(entry);
        await db.SaveChangesAsync();
        return Result.Ok($"Posted expense JE #{entry.Id}");
    }
}
