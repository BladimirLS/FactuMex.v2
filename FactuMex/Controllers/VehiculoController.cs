using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renomax.Models;

namespace Renomax.Controllers
{
    [Route("apiv1/vehiculo")]
    [ApiController]

    public class VehiculosController : ControllerBase
    {
        [AuthRequired]
        public DataTable Get()
        {
            string IDOperacion = Request.Query.Where(v => v.Key == "IDOperacion").SingleOrDefault().Value;
            if (IDOperacion.Contains("C"))
            {
                string CodPoliza = IDOperacion.Substring(1);
                IDOperacion = new DB(Cns.Inb, "CargaUltOperacionRN", new List<object> { CodPoliza }.ToArray()).EjecutaCmd().ToString();
            }
            return new DB(Cns.Inb, "CargaVehiculo", new List<object> { IDOperacion }.ToArray())
                .RetornaDT();
        }
    }
}