using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class DTOCampo
    {
        public string Origen{get;set;}
        public string Destino { get; set; }
        public string Plantilla { get; set; }
    }
    public class DTOCampos
    {
        public DTOCampo[] Campos { get; set; }
        public string TablaOrigen { get; set; }
        public string TipoActualizacion { get; set; }
    }
}
