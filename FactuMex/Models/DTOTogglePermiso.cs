using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class DTOTogglePermiso
    {
        public long IDUsuario { get; set; }
        public string Permiso { get; set; }
        public bool Estatus { get; set; }
    }
}

