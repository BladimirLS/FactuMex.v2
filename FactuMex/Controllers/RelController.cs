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
    [Route("apiv1/rel")]
    [ApiController]
    public class RelController : ControllerBase
    {
        [HttpPost]
        [AuthRequired]
        public int Post(Rels Relaciones)
        {
            new DB(Cns.Renomax, "GuardaRels", new List<object>() { Relaciones }.ToArray());
            return 0;
        }
        [HttpGet]
        [AuthRequired]
        public DataTable Get()
        {
            return new DB(Cns.Renomax, "CargaRel", new List<object>() { }.ToArray()).RetornaDTxP() ;
        }
        [HttpDelete]
        public int Delete(DTORel Relacion)
        {
            new DB(Cns.Renomax, "BorraRel", new List<object>() { Relacion.Plantilla }.ToArray()).EjecutaCmd();
            return 0;
        }
    }
}