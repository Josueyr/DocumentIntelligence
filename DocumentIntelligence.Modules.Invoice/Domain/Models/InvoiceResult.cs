using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentIntelligence.Modules.Invoice.Domain.Models
{
    public class InvoiceResult
    {
        public string AmountDue { get; set; } = string.Empty; // Monto total (Amount Due)
        public string BillingAddress { get; set; } = string.Empty; // Dirección del proveedor / facturación (Billing address)
        public string BillingAddressRecipient { get; set; } = string.Empty; // Destinatario de la dirección de facturación (Billing address recipient)
        public string CustomerAddress { get; set; } = string.Empty; // Dirección del cliente (Customer address)
        public string CustomerAddressRecipient { get; set; } = string.Empty; // Destinatario de la dirección del cliente (Customer address recipient)
        public string CustomerId { get; set; } = string.Empty; // Identificación del cliente (Customer ID)
        public string CustomerName { get; set; } = string.Empty; // Nombre del cliente (Customer name)
        public string DueDate { get; set; } = string.Empty; // Fecha de vencimiento (Due date)
        public string InvoiceDate { get; set; } = string.Empty; // Fecha de la factura (Invoice date)
        public string InvoiceId { get; set; } = string.Empty; // Identificador de la factura (Invoice ID)
        public string InvoiceTotal { get; set; } = string.Empty; // Total de la factura (Invoice total)
        public string PreviousUnpaidBalance { get; set; } = string.Empty; // Saldo pendiente anterior (Previous unpaid balance)
        public string PurchaseOrder { get; set; } = string.Empty; // Orden de compra (Purchase order)
        public string RemittanceAddress { get; set; } = string.Empty; // Dirección de remisión (Remittance address)
        public string RemittanceAddressRecipient { get; set; } = string.Empty; // Destinatario de la dirección de remisión (Remittance address recipient)
        public string ServiceAddress { get; set; } = string.Empty; // Dirección del servicio (Service address)
        public string ServiceAddressRecipient { get; set; } = string.Empty; // Destinatario de la dirección del servicio (Service address recipient)
        public string ServiceEndDate { get; set; } = string.Empty; // Fecha de fin del servicio (Service end date)
        public string ServiceStartDate { get; set; } = string.Empty; // Fecha de inicio del servicio (Service start date)
        public string ShippingAddress { get; set; } = string.Empty; // Dirección de envío (Shipping address)
        public string ShippingAddressRecipient { get; set; } = string.Empty; // Destinatario de la dirección de envío (Shipping address recipient)
        public string SubTotal { get; set; } = string.Empty; // Subtotal (Subtotal)
        public string TaxDetails { get; set; } = string.Empty; // Detalles de impuestos (Tax details)
        public string TotalTax { get; set; } = string.Empty; // Impuestos totales (Total tax)
        public string VendorAddress { get; set; } = string.Empty; // Dirección del proveedor (Vendor address)
        public string VendorAddressRecipient { get; set; } = string.Empty; // Destinatario de la dirección del proveedor (Vendor address recipient)
        public string VendorName { get; set; } = string.Empty; // Nombre del proveedor (Vendor name)


        public InvoiceResult() { }
    }
}
