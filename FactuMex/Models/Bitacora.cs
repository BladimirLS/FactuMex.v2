using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Bitacora
    {
        public long Idbitacora { get; set; }
        public string Entidad { get; set; }
        public string Accion { get; set; }
        public long? Pk { get; set; }
        public long? Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string Metadata { get; set; }
    }
}
