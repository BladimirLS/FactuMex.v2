using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Ncf100
    {
        public long Id { get; set; }
        public string Secuencial { get; set; }
        public long? Idfactura { get; set; }
    }
}
