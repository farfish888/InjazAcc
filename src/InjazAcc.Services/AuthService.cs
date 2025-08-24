using InjazAcc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InjazAcc.Services;

public class AuthService(AppDbContext db)
{
    public async Task<bool> ValidateAsync(string username, string password)
    {
        var u = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        return u != null;
    }
}
