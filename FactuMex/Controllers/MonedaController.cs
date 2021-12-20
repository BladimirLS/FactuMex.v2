using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.DBModel;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Renomax.Models;

namespace Renomax.Controllers
{
    [Route("apiv1/moneda")]
    [ApiController]
    public class MonedaController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public IEnumerable<dynamic> Get()
        {

            return new FactuMexContext().Moneda.ToList();
        }
    }
}