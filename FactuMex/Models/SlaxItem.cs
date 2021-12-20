using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class SlaxItem
    {
        public long Idsla { get; set; }
        public string TipoSla { get; set; }
        public long? Iditem { get; set; }
        public long? Idprovincia { get; set; }
        public double? Duracion { get; set; }
        public string Und { get; set; }
        public bool? NotificarExc { get; set; }
        public string Correo { get; set; }
    }
}
