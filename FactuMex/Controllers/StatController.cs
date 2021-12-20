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
    [Route("apiv1/stat")]
    [ApiController]
    public class StatController : ControllerBase
    {
        [HttpGet]
        public DataSet Get()
        {
            DataSet ds = new DataSet("Stats");
            ds.Tables.Add(
            new DB(Cns.Renomax, "CargaStatEnvios", null).RetornaDT());
            ds.Tables[0].TableName = "Envios";
            ds.Tables.Add(
            new DB(Cns.Renomax, "CargaStatReno", null).RetornaDT());
            ds.Tables[1].TableName = "Reno";
            return ds;
        }
    }
}