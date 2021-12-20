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
    [Route("apiv1/imprimefac")]
    [ApiController]
    public class ImprimeFacController : ControllerBase
    {
        [HttpPost]
        [AuthRequired]
        public long Post(DTOValxFac e)
        {
            var db = new FactuMexContext();
            db.ColaImpresion.Add(new ColaImpresion { Fhcola=DateTime.Now,Idfac=e.fac,Ubicacion="default"});
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Imprimir",
                    Entidad = "Factura",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e),
                    Pk = e.fac,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });

            db.SaveChanges();
            return e.fac;
        }
    }
}