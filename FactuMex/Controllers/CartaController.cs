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
    [Route("apiv1/carta")]
    [ApiController]
    public class CartaController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public DataTable Get()
        {
            string s = Request.Query.Where(v => v.Key == "s").SingleOrDefault().Value;
            if (s == "p")
                return new DB(Cns.Renomax, "CargaEnviosPend", null).RetornaDT();
            if (s == "n")
            {
                string nombre = Request.Query.Where(v => v.Key == "nombre").SingleOrDefault().Value;
                return new DB(Cns.Renomax, "CargaEnviosPendxNombre", new List<object> { nombre }.ToArray() ).RetornaDTxP();
            }
            return new DataTable();
        }
    }
}