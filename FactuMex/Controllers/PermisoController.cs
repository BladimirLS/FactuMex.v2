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
    [Route("apiv1/permiso")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public dynamic Get()
        {
            string IDUsuario = Request.Query.Where(k => k.Key == "idusuario").SingleOrDefault().Value;
            DataTable usr= new DB(Cns.Renomax, "CargaPermisosUsuario", new List<object> { IDUsuario }.ToArray())
                .RetornaDTxP();
            DataTable plantilla = new DB(Cns.Renomax, "CargaPermisosPlantilla", new List<object> { IDUsuario }.ToArray())
                .RetornaDTxP();
            return new { Usuario = usr, Plantilla = plantilla }
;        }
        [HttpPost]
        [AuthRequired]
        public object Post(DTOTogglePermiso Permiso)
        {
            return new DB(Cns.Renomax, "GuardaPermisosUsuario", new List<object> { Permiso }.ToArray())
                .EjecutaCmd();
            ;
        }
    }
}