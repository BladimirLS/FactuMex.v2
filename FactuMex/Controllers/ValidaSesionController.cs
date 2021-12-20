using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Renomax.Controllers
{
    [Route("apiv1/validasesion")]
    [ApiController]
    public class ValidaSesionController : ControllerBase
    {
        [HttpPost]
        public dynamic Put(DTOSesion SS)
        {
            string UA = Request.Headers["User-Agent"].ToString() + "|" + SS.Meta;
            SS.Meta = UA;
            return new { denied = new DB(Cns.Renomax, "ValidaSesion", new List<object> { SS }.ToArray()).EjecutaCmd().ToString() };
        }
    }
}