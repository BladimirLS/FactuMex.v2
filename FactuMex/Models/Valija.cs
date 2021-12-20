using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Valija
    {
        public long Idvalija { get; set; }
        public string Etiqueta { get; set; }
        public DateTime? Fecha { get; set; }
        public string Cb { get; set; }
        public string Cs { get; set; }
        public string Usuario { get; set; }
    }
}
