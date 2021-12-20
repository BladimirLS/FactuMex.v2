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
    [Route("apiv1/localidad")]
    [ApiController]
    public class LocalidadController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public IEnumerable<dynamic> Get()
        {
            string search = Request.Query.Where(v => v.Key == "s").SingleOrDefault().Value;
            switch (search)
            {
                case "p":
                    return new FactuMexContext().Provincia.ToList();
                case "m":
                    return new FactuMexContext().Municipio.ToList();
                case "s":
                    return new FactuMexContext().Sector.ToList();
            }
            return new FactuMexContext().Provincia.ToList();
        }
    }
}