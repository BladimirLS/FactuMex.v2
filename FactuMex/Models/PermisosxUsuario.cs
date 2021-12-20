using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class PermisosxUsuario
    {
        public long IdpermisoxUsuario { get; set; }
        public long? Idusuario { get; set; }
        public string Permiso { get; set; }
    }
}
