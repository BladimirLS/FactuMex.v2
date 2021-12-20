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
    [Route("apiv1/mail")]
    [ApiController]
    public class MailController : ControllerBase
    {
        [HttpPost]
        [AuthRequired]
        public bool Post(Poliza P)
        {
            new DB(Cns.Renomax, "EnviaMail", new List<object>() { P.CodPoliza }.ToArray()).EjecutaCmd();
            return new ServiceCall("EnviarCarta", P.CodPoliza).Call();
        }
        [HttpGet]
        [AuthRequired]
        public DataTable Get()
        {
            string Poliza = Request.Query.Where(v => v.Key == "Poliza").SingleOrDefault().Value;
            return new DB(Cns.Renomax, "CargaCorreo", new List<object>() { Poliza }.ToArray()).RetornaDTxP();
        }
    }
}