using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CallCenter.DBModel;
using Renomax.Models;

namespace CallCenter.Controllers
{
    
    [ApiController]
    public class RNCController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        [Route("apiv1/rnc/{r?}")]
        public Rncempresas Get(string r)
        {
            return new FactuMexContext().Rncempresas.Where(e => e.Rnc == r).FirstOrDefault();
        }
    }
}