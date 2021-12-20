using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class Usuario
    {
        public string IDUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Foto { get; set; }
        public string Borrado { get; set; }
    }
}
