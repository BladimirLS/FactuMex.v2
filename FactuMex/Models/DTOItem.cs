using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.DBModel;
namespace CallCenter.Models
{
    public class DTOItem
    {
        public Item Item {get;set;}
        public List<PrecioxItem> PrecioItem { get; set; }
        public List<SlaxItem> SLAItem { get; set; }
    }
}
