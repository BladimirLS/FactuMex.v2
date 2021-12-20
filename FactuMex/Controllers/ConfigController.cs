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
    [Route("apiv1/config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpPost]
        [AuthRequired]
        public int Post(DTOConfig conf)
        {
            new DB(Cns.Renomax, "GuardaConfig", new List<object> { conf.ano }.ToArray()).EjecutaCmd();
            conf.anexos.ToList().ForEach((Anexo a) =>
            {
                if (a.Arc.Base64Url.Length > 50)
                    new DB(Cns.Renomax, "GuardaAnexo", new List<object> { a }.ToArray()).EjecutaCmd();
            });
            return 0;
        }
        [HttpGet][AuthRequired]
        public DataTable Get()
        {
            bool ano = Request.Query.Where(p => p.Key == "ano").Count() == 1;
            if (!ano)
                return new DB(Cns.Renomax, "CargaAnexos", null).RetornaDT();
            else
            {
                return new DB(Cns.Renomax, "CargaAno", null).RetornaDT();
            }

        }
        [HttpDelete]
        public int Delete()
        {
            string i = Request.Query.Where(v => v.Key == "i").SingleOrDefault().Value;
            new DB(Cns.Renomax, "BorraAnexo", new List<object> { i }.ToArray()).EjecutaCmd();
            return 0;
        }
    }
}