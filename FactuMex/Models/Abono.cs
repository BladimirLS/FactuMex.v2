using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Abono
    {
        public long Idabono { get; set; }
        public long? Idfactura { get; set; }
        public double? Pagado { get; set; }
    }
}
