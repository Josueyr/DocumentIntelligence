using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentIntelligence.Modules.Receipt.Domain.Models
{
    public class AmountModel
    {
        public double Amount { get; set; } //Valor
        public string CurrencyCode { get; set; } = string.Empty; //Moneda
    }
}
