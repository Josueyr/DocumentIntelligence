using DocumentIntelligence.Modules.Receipt.Domain.Models;

public class ReceiptResult
{
    public string ReceiptType { get; } //TipoRecibo
    public string CountryRegion { get; } //RegionRecibo
    public string MerchantAddress { get; } //DireccionComercio
    public string MerchantName { get; } //NombreComercio
    public string MerchantCif { get; } //CIFComercio
    public TaxModel TaxDetails { get; } //DetallesImpuestos
    public AmountModel Total { get; } //ImporteBruto
    public string TransactionDate { get; } //FechaRecibo
    public string TransactionTime { get; } //HoraRecibo
    public string InvoiceId { get; } //NumeroFacturaRecibo

    public ReceiptResult(
        string receiptType,
        string countryRegion,
        string merchantAddress,
        string merchantName,
        string merchantCif,
        TaxModel taxDetails,
        AmountModel total,
        string transactionDate,
        string transactionTime,
        string invoiceId)
    {
        ReceiptType = receiptType ?? throw new ArgumentNullException(nameof(receiptType));
        CountryRegion = countryRegion ?? throw new ArgumentNullException(nameof(countryRegion));
        MerchantAddress = merchantAddress ?? string.Empty;
        MerchantName = merchantName ?? string.Empty;
        MerchantCif = merchantCif ?? string.Empty;
        TaxDetails = taxDetails ?? throw new ArgumentNullException(nameof(taxDetails));
        Total = total ?? throw new ArgumentNullException(nameof(total));
        TransactionDate = transactionDate ?? string.Empty;
        TransactionTime = transactionTime ?? string.Empty;
        InvoiceId = invoiceId ?? string.Empty;
    }
}
