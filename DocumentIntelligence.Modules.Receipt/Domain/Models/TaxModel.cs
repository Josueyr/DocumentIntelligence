using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentIntelligence.Modules.Receipt.Domain.Models
{
    public class TaxModel
    {
        public AmountModel Amount { get; set; } = new(); //Impuesto
        public string Description { get; set; } = string.Empty; //TipoImpuesto
        public AmountModel NetAmount { get; set; } = new(); //ImporteNeto
        public string Rate { get; set; } = string.Empty; //PorcentajeImpuestoAplicado
    }
}
