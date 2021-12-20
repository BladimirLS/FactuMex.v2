using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Factura
    {
        public long Idfactura { get; set; }
        public string Ncf { get; set; }
        public long? Idcliente { get; set; }
        public DateTime? Fecha { get; set; }
        public string Tipo { get; set; }
        public long? Idempresa { get; set; }
        public string FormaPago { get; set; }
        public string MetodoPago { get; set; }
    }
}
