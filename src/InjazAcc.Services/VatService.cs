using InjazAcc.Shared;

namespace InjazAcc.Services;

public static class VatService
{
    public static (decimal net, decimal vat, decimal gross) Compute(decimal amount, decimal? vatRate = null)
    {
        var r = vatRate ?? Settings.DefaultVatRate;
        var vat = Math.Round(amount * r, 2, MidpointRounding.AwayFromZero);
        var gross = Math.Round(amount + vat, 2, MidpointRounding.AwayFromZero);
        return (amount, vat, gross);
    }
}
