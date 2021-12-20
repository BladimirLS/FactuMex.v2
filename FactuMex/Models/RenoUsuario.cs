using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class RenoUsuario
    {
        public long Idusuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Foto { get; set; }
        public bool? Borrado { get; set; }
    }
}
