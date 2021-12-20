using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class PrecioxItem
    {
        public long Idprecio { get; set; }
        public long? Idprovincia { get; set; }
        public long? Iditem { get; set; }
        public string Moneda { get; set; }
        public double? Precio { get; set; }
    }
}
