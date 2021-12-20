using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Sector
    {
        public double Idlocalidad { get; set; }
        public string Localidad { get; set; }
        public double? Cp { get; set; }
        public double? Zona { get; set; }
        public double? Idmunicipio { get; set; }
    }
}
