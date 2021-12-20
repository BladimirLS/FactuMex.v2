using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class DesgloseFactura
    {
        public long Iddesglose { get; set; }
        public string Concepto { get; set; }
        public string Producto { get; set; }
        public long? Idprovincia { get; set; }
        public long? Cantidad { get; set; }
        public string Moneda { get; set; }
        public double? Tarifa { get; set; }
        public double? Monto { get; set; }
        public long? Idfactura { get; set; }
    }
}
