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
    [Route("apiv1/operacion")]
    [ApiController]

    public class OperacionController : ControllerBase
    {
        [AuthRequired]
        public DataTable Get()
        {
            string IDCliente = Request.Query.Where(v => v.Key == "IDCliente").SingleOrDefault().Value;
            return new DB(Cns.Inb,"CargaOperacion", new List<object> { IDCliente }.ToArray())
                .RetornaDT();
        }
    }
}