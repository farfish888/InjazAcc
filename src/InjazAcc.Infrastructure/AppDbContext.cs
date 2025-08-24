using Microsoft.EntityFrameworkCore;
using InjazAcc.Domain;

namespace InjazAcc.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<CostCenter> CostCenters => Set<CostCenter>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalLine> JournalLines => Set<JournalLine>();
    public DbSet<ExpenseVoucher> Expenses => Set<ExpenseVoucher>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Account>().HasIndex(x => x.Code).IsUnique();
        b.Entity<Invoice>().HasMany(i => i.Lines).WithOne().HasForeignKey(l => l.InvoiceId);
        b.Entity<JournalEntry>().HasMany(e => e.Lines).WithOne().HasForeignKey(l => l.EntryId);
    }
}

