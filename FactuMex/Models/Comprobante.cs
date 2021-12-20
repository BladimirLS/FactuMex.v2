using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Comprobante
    {
        public long Idcomp { get; set; }
        public string EstatDesde { get; set; }
        public long? NumDesde { get; set; }
        public string EstatHasta { get; set; }
        public long? NumHasta { get; set; }
        public DateTime? Venc { get; set; }
        public string TipoComp { get; set; }
        public string Rnc { get; set; }
        public string Empresa { get; set; }
    }
}
