using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Incompleto
    {
        public long Idinc { get; set; }
        public long? Idfac { get; set; }
        public long? Idval { get; set; }
        public string Relacionado { get; set; }
        public bool? Despachado { get; set; }
        public bool? Recibido { get; set; }
        public DateTime? FecDsp { get; set; }
        public DateTime? FecRec { get; set; }
    }
}
