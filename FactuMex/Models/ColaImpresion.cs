using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class ColaImpresion
    {
        public long Idprint { get; set; }
        public long? Idfac { get; set; }
        public string Ubicacion { get; set; }
        public DateTime? Fhcola { get; set; }
        public DateTime? Fhimpresion { get; set; }
    }
}
