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
    [Route("apiv1/plantilla")]
    [ApiController]
    public class PlantillaController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public DataTable Get() {
            return new DB(Cns.Renomax, "CargaPlantillas", new List<string>().ToArray()).RetornaDT();
        }
        [HttpPost]
        [AuthRequired]
        public object Post(DTOSelPlantilla Seleccion)
        {
            return new DB(Cns.Renomax, "GuardaSeleccionPlantilla", new List<object>() {Seleccion }.ToArray()).EjecutaCmd();
        }
    }
}