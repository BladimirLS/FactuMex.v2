using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Sesion
    {
        public long Idsesion { get; set; }
        public string CodSesion { get; set; }
        public long? Idusuario { get; set; }
        public DateTime? Expira { get; set; }
        public string Bfp { get; set; }
    }
}
