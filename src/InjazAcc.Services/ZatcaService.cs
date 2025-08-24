using System.Security.Cryptography;
using System.Text;

namespace InjazAcc.Services;

public static class ZatcaService
{
    // Minimal TLV for simplified invoice QR (not signed)
    public static string BuildQrBase64(string sellerName, string vatNumber, DateTime dt, decimal total, decimal vatAmount)
    {
        byte[] Tlv(byte tag, string value)
        {
            var vBytes = Encoding.UTF8.GetBytes(value);
            return [tag, (byte)vBytes.Length, .. vBytes];
        }
        var payload = Tlv(1, sellerName)
            .Concat(Tlv(2, vatNumber))
            .Concat(Tlv(3, dt.ToString("s")))
            .Concat(Tlv(4, total.ToString("0.00")))
            .Concat(Tlv(5, vatAmount.ToString("0.00"))).ToArray();
        return Convert.ToBase64String(payload);
    }

    public static string ComputeHash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    // Very lightweight UBL-like XML (placeholder, not full compliance)
    public static string BuildUblXml(string uuid, string sellerName, string sellerVat, string buyerName, string buyerVat, DateTime date, decimal net, decimal vat, decimal total)
    {
        return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<Invoice xmlns=""urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"">
  <cbc:ID xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"">{uuid}</cbc:ID>
  <cbc:IssueDate xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"">{date:yyyy-MM-dd}</cbc:IssueDate>
  <cac:AccountingSupplierParty xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"">
    <cac:Party><cac:PartyName><cbc:Name>{sellerName}</cbc:Name></cac:PartyName><cac:PartyTaxScheme><cbc:CompanyID>{sellerVat}</cbc:CompanyID></cac:PartyTaxScheme></cac:Party>
  </cac:AccountingSupplierParty>
  <cac:AccountingCustomerParty xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"">
    <cac:Party><cac:PartyName><cbc:Name>{buyerName}</cbc:Name></cac:PartyName><cac:PartyTaxScheme><cbc:CompanyID>{buyerVat}</cbc:CompanyID></cac:PartyTaxScheme></cac:Party>
  </cac:AccountingCustomerParty>
  <cac:LegalMonetaryTotal xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"">
    <cbc:TaxExclusiveAmount xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"">{net:0.00}</cbc:TaxExclusiveAmount>
    <cbc:TaxInclusiveAmount xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"">{total:0.00}</cbc:TaxInclusiveAmount>
    <cbc:PayableAmount xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"">{total:0.00}</cbc:PayableAmount>
  </cac:LegalMonetaryTotal>
  <cac:TaxTotal xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"">
    <cbc:TaxAmount xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"">{vat:0.00}</cbc:TaxAmount>
  </cac:TaxTotal>
</Invoice>";
    }
}
