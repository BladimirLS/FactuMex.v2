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
    [Route("apiv1/pago")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public DataTable Get()
        {
            string b = Request.Query.Where(v => v.Key == "b").SingleOrDefault().Value;
            return new DB(Cns.MetodosPago, "BuscaMetodoPago", new List<object> { b }.ToArray()).RetornaDTxP();
        }
        [HttpPost]
        [AuthRequired]
        public int Post(DTOPago pago)
        {
            new DB(Cns.MetodosPago, "CambiaMetodoPago", new List<object> { pago.Puid,pago.Metodo }.ToArray()).EjecutaCmd();
            return 0;
        }
    }
}