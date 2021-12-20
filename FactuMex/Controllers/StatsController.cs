using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Renomax.Controllers
{
    [Route("apiv1/statd")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        public DataTable Get()
        {
            string r = Request.Query.Where(v => v.Key == "c").SingleOrDefault().Value;
            return new DB(Cns.Renomax, r, null).RetornaDT();
        }
    }
}