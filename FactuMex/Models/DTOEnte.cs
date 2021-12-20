using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.DBModel;
namespace CallCenter.Models
{
    public class DTOEnte
    {
        public Ente Ente {get;set;}
        public DirxEnte DirEntrega { get; set; }
        public DirxEnte DirFacturacion { get; set; }
    }
}
