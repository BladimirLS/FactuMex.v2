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

    [ApiController]
    public class AnexoController : ControllerBase
    {
        [HttpGet]
        [Route("apiv1/descanexo/{f?}")]
        public FileResult Get(string f)
        {
            DataTable dt = new DB(Cns.Renomax, "DescargaAnexo", new List<object> { f }.ToArray()).RetornaDTxP();
            object[] Vals = dt.Rows[0].ItemArray;
            string Nombre = Vals[1].ToString();
            byte[] Contenido = (byte[])Vals[2];
            return File(Contenido, "application/octet-stream", Nombre);
        }
    }
}