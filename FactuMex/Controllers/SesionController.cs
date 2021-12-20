using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Renomax.Controllers
{
    [Route("apiv1/sesion")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        [HttpGet]
        public dynamic Get()
        {
            string email = Request.Query.Where(u => u.Key == "email").SingleOrDefault().Value;
            DataTable user = new DB(Cns.Renomax, "CargaUsuariosxEmail", new List<object> { email }.ToArray()).RetornaDTxP();
            DataTable permits = new DB(Cns.Renomax, "CargaPermisosxEmail", new List<object> { email }.ToArray()).RetornaDTxP();
            return new { Usuario=user, Permisos=permits };
        }
        [HttpPost]
        public DataTable Post(DTOIniciaSesion SS)
        {
            string UA = Request.Headers["User-Agent"].ToString()+"|"+SS.Meta;
            DataTable SSDT = new DataTable();
            if (new DB(Cns.Renomax, "ValidaUsuario", new List<object> { SS }.ToArray())
                .EjecutaCmd().ToString() == "SI")
            {
                long Sesion = Convert.ToInt32(new DB(Cns.Renomax, "IniciaSesion", new List<object> { SS.Email,UA }.ToArray())
                 .EjecutaCmd());
                SSDT = new DB(Cns.Renomax, "RetornaSesion", new List<object> { Sesion }.ToArray()).RetornaDTxP();
            }

            return SSDT;
        }
        
    }
}