using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Renomax.Models
{
    public class Archivo
    {
        public long IDArchivo { get; set; }
        public string BinaryString { get; set; }
        public string Base64Url { get; set; }
        public string MimeType { get; set; }
        public string Nombre { get; set; }
        public long IDDescripcion { get; set; }
        public long Tamano { get; set; }
        public long IDEntidad { get; set; }
        public long PKEntidad { get; set; }
        public string Ruta { get; set; }
    }
    public class ListArchivos
    {
        public Archivo[] Archivos { get; set; }
    }
}
