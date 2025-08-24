using InjazAcc.Domain;
using InjazAcc.Shared;
using Microsoft.EntityFrameworkCore;

namespace InjazAcc.Infrastructure;

public static class DbInitializer
{
    public static async Task EnsureSeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (!await db.Accounts.AnyAsync())
        {
            var accounts = new[] {
                new Account{ Code="1000", Name="الصندوق", Type=AccountType.Asset },
                new Account{ Code="1010", Name="البنك", Type=AccountType.Asset },
                new Account{ Code="1100", Name="العملاء", Type=AccountType.Asset },
                new Account{ Code="1140", Name="VAT مدخلات", Type=AccountType.Asset },
                new Account{ Code="2130", Name="VAT مخرجات", Type=AccountType.Liability },
                new Account{ Code="3200", Name="جاري المالك", Type=AccountType.Equity },
                new Account{ Code="4100", Name="إيرادات خدمات الحراسة", Type=AccountType.Revenue },
                new Account{ Code="5100", Name="مصروفات تشغيل", Type=AccountType.Expense },
            };
            await db.Accounts.AddRangeAsync(accounts);
        }

        if (!await db.Branches.AnyAsync())
        {
            await db.Branches.AddRangeAsync(
                new Branch{ Name = "المقر الرئيسي" },
                new Branch{ Name = "فرع المدينة" }
            );
        }

        if (!await db.Users.AnyAsync())
        {
            await db.Users.AddRangeAsync(
                new AppUser { Username="admin", Password="1234", Role=UserRole.Accountant },
                new AppUser { Username="super", Password="1234", Role=UserRole.Supervisor },
                new AppUser { Username="read",  Password="1234", Role=UserRole.Reader }
            );
        }

        await db.SaveChangesAsync();
    }
}
