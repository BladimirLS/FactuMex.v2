using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renomax.Models;

namespace Renomax.Controllers
{
    [Route("apiv1/asignacion")]
    [ApiController]
    public class AsignacionController : ControllerBase
    {
        [HttpPost]
        [AuthRequired]
        public object Post(DTOAsignacion asig)
        {
            return new DB(Cns.Inb, "GuardaAsignacion", new List<object>() { asig }.ToArray()).EjecutaCmd();
        }
    }
}