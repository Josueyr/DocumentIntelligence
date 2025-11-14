using Azure.AI.DocumentIntelligence;
using DocumentIntelligence.Common.Models;
using DocumentIntelligence.Modules.Invoice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentIntelligence.Modules.Invoice.Infrastructure.Services.Mappers
{
    // Mapper para convertir la respuesta del servicio de Azure Document Intelligence
    // a nuestro modelo interno `InvoiceResult`.
    // Toma los campos que la IA detecta en la factura
    // y los normaliza para que el resto de la app pueda manejarlos fácilmente.
    public static class AzureInvoiceMapper
    {
        private const float MIN_CONFIDENCE = 0.85f;

        // Convierte la lista de documentos analizados en una lista de resultados de factura.
        // Simplemente itera los documentos y llama a la función que crea cada resultado.
        public static OperationResult<List<InvoiceResult>> MapFrom(IReadOnlyList<AnalyzedDocument> documents)
        {
            List<InvoiceResult> listInvoiceResult = new();

            foreach (AnalyzedDocument doc in documents)
            {
                listInvoiceResult.Add(CreateInvoiceResult(doc));
            }

            return OperationResult<List<InvoiceResult>>.Success(listInvoiceResult);
        }

        // Crea el objeto `InvoiceResult` por cada documento analizado.
        // Aquí se mapean los nombres de campo reconocidos por Azure a las propiedades de nuestro modelo.
        private static InvoiceResult CreateInvoiceResult(AnalyzedDocument doc)
        {
            return new InvoiceResult
            {
                InvoiceId = GetValueFromKey(doc, "InvoiceId"),
                InvoiceDate = GetValueFromKey(doc, "InvoiceDate"),
                InvoiceTotal = GetValueFromKey(doc, "InvoiceTotal"),
                AmountDue = GetValueFromKey(doc, "AmountDue"),
                VendorName = GetValueFromKey(doc, "VendorName"),
                VendorAddress = GetValueFromKey(doc, "VendorAddress"),
                VendorAddressRecipient = GetValueFromKey(doc, "VendorAddressRecipient"),
                CustomerName = GetValueFromKey(doc, "CustomerName"),
                CustomerId = GetValueFromKey(doc, "CustomerId"),
                CustomerAddress = GetValueFromKey(doc, "CustomerAddress"),
                CustomerAddressRecipient = GetValueFromKey(doc, "CustomerAddressRecipient"),
                SubTotal = GetValueFromKey(doc, "SubTotal"),
                TotalTax = GetValueFromKey(doc, "TotalTax"),
                TaxDetails = GetTaxDetailsFromKey(doc, "TaxDetails"),
                DueDate = GetValueFromKey(doc, "DueDate"),
                PurchaseOrder = GetValueFromKey(doc, "PurchaseOrder"),
                RemittanceAddress = GetValueFromKey(doc, "RemittanceAddress"),
                RemittanceAddressRecipient = GetValueFromKey(doc, "RemittanceAddressRecipient"),
                ServiceStartDate = GetValueFromKey(doc, "ServiceStartDate"),
                ServiceEndDate = GetValueFromKey(doc, "ServiceEndDate"),
                ServiceAddress = GetValueFromKey(doc, "ServiceAddress"),
                ServiceAddressRecipient = GetValueFromKey(doc, "ServiceAddressRecipient"),
                BillingAddress = GetValueFromKey(doc, "BillingAddress"),
                BillingAddressRecipient = GetValueFromKey(doc, "BillingAddressRecipient"),
                ShippingAddress = GetValueFromKey(doc, "ShippingAddress"),
                ShippingAddressRecipient = GetValueFromKey(doc, "ShippingAddressRecipient"),
                PreviousUnpaidBalance = GetValueFromKey(doc, "PreviousUnpaidBalance")
            };
        }

        // Obtiene un valor simple por clave. Respeta un umbral mínimo de confianza
        // y devuelve el contenido tal cual si la confianza es baja pero existe texto.
        private static string GetValueFromKey(AnalyzedDocument doc, string key)
        {
            if (doc == null) return string.Empty;

            if (doc.Fields.TryGetValue(key, out DocumentField documentField))
            {
                if (documentField.Confidence.HasValue && documentField.Confidence.Value >= MIN_CONFIDENCE)
                {
                    return ExtractValueByType(documentField);
                }

                if (!string.IsNullOrEmpty(documentField.Content))
                    return documentField.Content;
            }

            return string.Empty;
        }

        // Extrae los detalles de impuestos cuando Azure devuelve una lista/objeto.
        // Algunos documentos ofrecen un listado (array) con objetos por cada tipo de impuesto;
        // esta función intenta formatearlos en una cadena legible con descripción y cantidades.
        private static string GetTaxDetailsFromKey(AnalyzedDocument doc, string key)
        {
            if (doc == null) return string.Empty;

            if (!doc.Fields.TryGetValue(key, out DocumentField documentField))
                return string.Empty;

            var stringBuilder = new StringBuilder();

            if (documentField.FieldType == DocumentFieldType.List && documentField.ValueList != null && documentField.ValueList.Any())
            {
                foreach (var item in documentField.ValueList)
                {
                    if (item.ValueDictionary != null && item.ValueDictionary.Any())
                    {
                        var parts = new List<string>();

                        if (item.ValueDictionary.TryGetValue("Description", out DocumentField descField) && !string.IsNullOrEmpty(descField.Content))
                            parts.Add(descField.Content.Trim());

                        if (item.ValueDictionary.TryGetValue("Amount", out DocumentField amountField) && amountField.FieldType == DocumentFieldType.Currency && amountField.ValueCurrency != null)
                        {
                            var amt = amountField.ValueCurrency.Amount;
                            var code = amountField.ValueCurrency.CurrencyCode ?? string.Empty;
                            parts.Add($"{amt:0.##}{(string.IsNullOrEmpty(code) ? string.Empty : " " + code)}");
                        }

                        if (item.ValueDictionary.TryGetValue("NetAmount", out DocumentField netField) && netField.FieldType == DocumentFieldType.Currency && netField.ValueCurrency != null)
                        {
                            var n = netField.ValueCurrency.Amount;
                            var c = netField.ValueCurrency.CurrencyCode ?? string.Empty;
                            parts.Add($"Net: {n:0.##}{(string.IsNullOrEmpty(c) ? string.Empty : " " + c)}");
                        }

                        if (item.ValueDictionary.TryGetValue("Rate", out DocumentField rateField) && !string.IsNullOrEmpty(rateField.Content))
                            parts.Add($"Rate: {rateField.Content.Trim()}");

                        if (parts.Any())
                        {
                            stringBuilder.Append(string.Join(" - ", parts));
                        }
                        else if (!string.IsNullOrEmpty(item.Content))
                        {
                            stringBuilder.Append(item.Content.Trim());
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.Content))
                            stringBuilder.Append(item.Content.Trim());
                    }

                    if (stringBuilder.Length > 0)
                        stringBuilder.AppendLine();
                }

                return stringBuilder.ToString().Trim();
            }

            // Si no es una lista, llevamos a cabo la extracción normal
            if (documentField.Confidence.HasValue && documentField.Confidence.Value >= MIN_CONFIDENCE)
            {
                return ExtractValueByType(documentField);
            }

            if (!string.IsNullOrEmpty(documentField.Content))
                return documentField.Content;

            return string.Empty;
        }

        // Interpreta el tipo de campo devuelto por Azure y extrae una representación adecuada.
        // Por ejemplo: fechas, direcciones o monedas se convierten a texto legible.
        private static string ExtractValueByType(DocumentField documentField)
        {
            if (documentField == null) return string.Empty;

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

            return documentField.Content ?? string.Empty;
        }

        // Extrae la representación más amigable posible para un campo de dirección.
        // La IA puede devolver dirección como objeto o como texto; se intenta preferir
        // el texto ya formateado si existe.
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
