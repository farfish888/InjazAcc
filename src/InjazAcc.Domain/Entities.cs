using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InjazAcc.Shared;

namespace InjazAcc.Domain;

public class Account
{
    public int Id { get; set; }
    [MaxLength(20)] public string Code { get; set; } = string.Empty;
    [MaxLength(200)] public string Name { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public int? ParentId { get; set; }
    public bool IsLeaf { get; set; } = true;
}

public class Branch
{
    public int Id { get; set; }
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
}

public class CostCenter
{
    public int Id { get; set; }
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    public int BranchId { get; set; }
}

public class Site
{
    public int Id { get; set; }
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public int CostCenterId { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    [MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(30)] public string? TaxNumber { get; set; }
    public int? BranchId { get; set; }
}

public class AppUser
{
    public int Id { get; set; }
    [MaxLength(50)] public string Username { get; set; } = string.Empty;
    [MaxLength(256)] public string Password { get; set; } = string.Empty; // demo only
    public UserRole Role { get; set; } = UserRole.Accountant;
}

public class Invoice
{
    public int Id { get; set; }
    [MaxLength(30)] public string Number { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public int CustomerId { get; set; }
    public int? BranchId { get; set; }
    public int? CostCenterId { get; set; }
    public decimal TotalExclVat { get; set; }
    public decimal VatAmount { get; set; }
    public decimal TotalInclVat { get; set; }
    [MaxLength(64)] public string? Uuid { get; set; }
    [MaxLength(128)] public string? PrevHash { get; set; }
    public string? UblXmlPath { get; set; }
    public string? QrBase64 { get; set; }
    public List<InvoiceLine> Lines { get; set; } = new();
}

public class InvoiceLine
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    [MaxLength(200)] public string Description { get; set; } = string.Empty;
    public decimal Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; } = Settings.DefaultVatRate;
    public int? AccountId { get; set; } // revenue account
}

public class JournalEntry
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    [MaxLength(40)] public string Ref { get; set; } = string.Empty;
    public DocType DocType { get; set; }
    public int DocId { get; set; }
    public int? BranchId { get; set; }
    public int? CostCenterId { get; set; }
    [MaxLength(50)] public string CreatedBy { get; set; } = "system";
    public int? ReverseOfEntryId { get; set; }
    public List<JournalLine> Lines { get; set; } = new();
}

public class JournalLine
{
    public int Id { get; set; }
    public int EntryId { get; set; }
    public int AccountId { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Debit { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Credit { get; set; }
    [MaxLength(200)] public string? Note { get; set; }
}
public class ExpenseVoucher
{
    public int Id { get; set; }
    [MaxLength(30)] public string Number { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    [MaxLength(200)] public string Payee { get; set; } = string.Empty;
    public int AccountId { get; set; } // حساب المصروف
    public decimal AmountExclVat { get; set; }
    public decimal VatRate { get; set; } = InjazAcc.Shared.Settings.DefaultVatRate;
    public decimal VatAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentMethod Method { get; set; } = PaymentMethod.Cash; // Cash/Bank
    public int? BranchId { get; set; }
    public int? CostCenterId { get; set; }
}
