using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class Municipio
    {
        public double Idmunicipio { get; set; }
        public string Municipio1 { get; set; }
        public string Cp { get; set; }
        public string Zona { get; set; }
        public double? Idprovincia { get; set; }
    }
}
