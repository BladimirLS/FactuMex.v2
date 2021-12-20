using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.DBModel;
namespace CallCenter.Models
{
    public class DTOFac
    {
        public DTOEnte Cliente { get; set; }
        public Factura Fac { get; set; }
        public List<DesgloseFactura> Desglose { get; set; }
        public List<RefxFac> Tickets { get; set; }
        public List<RefxFac> Rels { get; set; }
        public RefxFac Referencia { get; set; }
        public float Pagado { get; set; }
        public string Stat { get; set; }
    }
}
