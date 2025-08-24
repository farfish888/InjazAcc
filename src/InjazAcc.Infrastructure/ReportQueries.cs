using InjazAcc.Shared;
using Microsoft.EntityFrameworkCore;

namespace InjazAcc.Infrastructure;

public record TrialBalanceRow(string Code, string Name, decimal Debit, decimal Credit, decimal Balance);

public static class ReportQueries
{
    public static async Task<List<TrialBalanceRow>> GetTrialBalanceAsync(AppDbContext db, DateTime from, DateTime to)
    {
        var rows = await db.JournalLines
            .Join(db.JournalEntries, l => l.EntryId, e => e.Id, (l, e) => new { l, e })
            .Where(x => x.e.Date >= from && x.e.Date <= to)
            .GroupBy(x => x.l.AccountId)
            .Select(g => new {
                AccountId = g.Key,
                Debit = g.Sum(x => x.l.Debit),
                Credit = g.Sum(x => x.l.Credit),
            }).ToListAsync();

        var accs = await db.Accounts.ToDictionaryAsync(a => a.Id);
        return rows.Select(r => new TrialBalanceRow(
            accs[r.AccountId].Code,
            accs[r.AccountId].Name,
            r.Debit,
            r.Credit,
            r.Debit - r.Credit
        )).OrderBy(x => x.Code).ToList();
    }
}
