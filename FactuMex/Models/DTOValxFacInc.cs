using CallCenter.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class DTOValxFacInc
    {
        public long val{ get; set; }
        public long fac { get; set; }
        public List<RefxFac> rel { get; set; }
        public string actn { get; set; }
    }
}
