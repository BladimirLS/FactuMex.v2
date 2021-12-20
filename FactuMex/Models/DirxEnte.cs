using System;
using System.Collections.Generic;

namespace CallCenter.DBModel
{
    public partial class DirxEnte
    {
        public long Iddireccion { get; set; }
        public long? Idente { get; set; }
        public string TipoDir { get; set; }
        public string Correo { get; set; }
        public string Calle { get; set; }
        public string Nro { get; set; }
        public string Edificio { get; set; }
        public string Apto { get; set; }
        public string Residencial { get; set; }
        public long? Idsector { get; set; }
        public string Sector { get; set; }
        public long? Idmunicipio { get; set; }
        public string Municipio { get; set; }
        public long? Idprovincia { get; set; }
        public string Provincia { get; set; }
        public short? Idpais { get; set; }
        public string Pais { get; set; }
    }
}
