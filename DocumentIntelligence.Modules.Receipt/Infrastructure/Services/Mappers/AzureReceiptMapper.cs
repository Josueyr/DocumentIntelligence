// Infrastructure/Mappers/AzureReceiptMapper.cs
using Azure.AI.DocumentIntelligence;
using DocumentIntelligence.Modules.Receipt.Domain.Models;

namespace DocumentIntelligence.Modules.Receipt.Infrastructure.Services.Mappers
{
    public static class AzureReceiptMapper
    {
        //MINIMO DE CONFIANZA QUE ESTABLECEMOS PARA DETECTAR QUE UN CAMPO OBTENIDO POR LA IA ES FIABLE
        private const float MIN_CONFIDENCE = 0.85f;

        public static OperationResult<List<ReceiptResult>> MapFrom(IReadOnlyList<AnalyzedDocument> documents)
        {
            List<ReceiptResult> listReceiptResult = new();

            foreach (AnalyzedDocument doc in documents)
            {
                listReceiptResult.Add(CreateReceiptResult(doc));
            }

            return OperationResult<List<ReceiptResult>>.Success(listReceiptResult);
        }

        private static ReceiptResult CreateReceiptResult(AnalyzedDocument doc)
        {
            return new ReceiptResult(
                receiptType: GetValueFromKey(doc, "ReceiptType"),
                countryRegion: GetValueFromKey(doc, "CountryRegion"),
                merchantAddress: GetValueFromKey(doc, "MerchantAddress"),
                merchantName: GetValueFromKey(doc, "MerchantName"),
                merchantCif: GetValueFromKey(doc, "MerchantCif"),
                taxDetails: GetTaxDetailFromKey(doc, "TaxDetails"),
                total: GetMonetaryAmountDetailFromKey(doc, "Total"),
                transactionDate: GetValueFromKey(doc, "TransactionDate"),
                transactionTime: GetValueFromKey(doc, "TransactionTime"),
                invoiceId: GetValueFromKey(doc, "InvoiceId")
            );
        }

        private static string GetValueFromKey(AnalyzedDocument doc, string key)
        {
            string result = string.Empty;
            if (doc.Fields.TryGetValue(key, out DocumentField documentField))
            {
                if (documentField.Confidence.HasValue && documentField.Confidence.Value >= MIN_CONFIDENCE)
                    result = ExtractValueByType(documentField);
            }
            return result;
        }

        private static TaxModel GetTaxDetailFromKey(AnalyzedDocument doc, string key)
        {
            TaxModel details = new();

            if (doc.Fields.TryGetValue(key, out DocumentField documentField))
            {
                if (documentField.ValueList.Any())
                {
                    //Hay casos en los que aparece la información repartida en varias filas
                    foreach (var val in documentField.ValueList)
                    {
                        if (val.Confidence.HasValue && val.Confidence.Value > MIN_CONFIDENCE)
                        {
                            if (val.ValueDictionary.TryGetValue("Amount", out DocumentField totalImpuesto))
                            {
                                details.Amount.Amount = totalImpuesto.ValueCurrency.Amount;
                                details.Amount.CurrencyCode = totalImpuesto.ValueCurrency.CurrencyCode;
                            }

                            if (val.ValueDictionary.TryGetValue("Description", out DocumentField tipoImpuesto))
                            {
                                details.Description = tipoImpuesto.Content;
                            }

                            if (val.ValueDictionary.TryGetValue("NetAmount", out DocumentField totalSinImpuesto))
                            {
                                details.NetAmount.Amount = totalSinImpuesto.ValueCurrency.Amount;
                                details.NetAmount.CurrencyCode = totalSinImpuesto.ValueCurrency.CurrencyCode;
                            }

                            if (val.ValueDictionary.TryGetValue("Rate", out DocumentField porcentajeImpuestoAplicado))
                            {
                                details.Rate = porcentajeImpuestoAplicado.Content;
                            }
                        }

                    }
                }
            }
            return details;
        }

        private static AmountModel GetMonetaryAmountDetailFromKey(AnalyzedDocument doc, string key)
        {
            AmountModel importe = new();

            if (doc.Fields.TryGetValue(key, out DocumentField documentField))
            {
                if (documentField.Confidence.HasValue && documentField.Confidence.Value >= MIN_CONFIDENCE)
                {
                    importe.Amount = documentField.ValueCurrency.Amount;
                    importe.CurrencyCode = documentField.ValueCurrency.CurrencyCode;
                }
            }
            return importe;
        }

        private static string ExtractValueByType(DocumentField documentField)
        {

            if (documentField.FieldType == DocumentFieldType.String)
            {
                return documentField.ValueString?.Replace("\n", " ") ?? string.Empty;
            }
            if (documentField.FieldType == DocumentFieldType.Date)
            {
                return documentField.ValueDate?.Date.ToShortDateString() ?? string.Empty;
            }
            if (documentField.FieldType == DocumentFieldType.CountryRegion)
            {
                return documentField.ValueCountryRegion ?? string.Empty;
            }
            if (documentField.FieldType == DocumentFieldType.Address)
            {
                return GetAddress(documentField);
            }
            if (documentField.FieldType == DocumentFieldType.Time)
            {
                return documentField.ValueTime?.ToString() ?? string.Empty;
            }
            if (documentField.FieldType == DocumentFieldType.Currency)
            {
                return documentField.ValueCurrency?.Amount.ToString() ?? string.Empty;
            }

            return string.Empty;
        }

        private static string GetAddress(DocumentField doc)
        {
            if (doc?.ValueAddress == null) return doc?.Content ?? string.Empty;

            if (!string.IsNullOrEmpty(doc.Content)) return doc.Content;

            if (!string.IsNullOrEmpty(doc.ValueAddress.House)) return doc.ValueAddress.House;

            if (!string.IsNullOrEmpty(doc.ValueAddress.StreetAddress)) return doc.ValueAddress.StreetAddress;

            return string.Empty;
        }
    }
}