using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.DBModel;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renomax.Models;

namespace Renomax.Controllers
{
    [Route("apiv1/cliente")]
    [ApiController]

    public class ClienteController : ControllerBase
    {
        [AuthRequired]
        public DataTable Get()
        {
            string Buscar = Request.Query.Where(v => v.Key == "Buscar").SingleOrDefault().Value;
            return new DB(Cns.Inb,"BuscaCliente", new List<object> { Buscar }.ToArray())
                .RetornaDT();
        }


    }
}