using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Anexos
    {
        public long Idanexo { get; set; }
        public string Nombre { get; set; }
        public byte[] Contenido { get; set; }
    }
}
