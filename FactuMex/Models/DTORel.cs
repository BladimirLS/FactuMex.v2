using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class DTORel
    {
        public string Origen{get;set;}
        public string Destino { get; set; }
        public string Plantilla { get; set; }
    }
    public class Rels
    {
        public DTORel[] Relaciones { get; set; }
    }
}
