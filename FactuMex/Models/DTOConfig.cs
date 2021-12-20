using Renomax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class DTOConfig
    {
        public int ano { get; set; }
        public Anexo[] anexos { get; set; }
    }
    public class Anexo
    {
        public Archivo Arc { get; set; }
        public int Index { get; set; }
    }
}
