using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CallCenter.DBModel;
using CallCenter.Models;
using Newtonsoft.Json;
using Renomax.Models;

namespace CallCenter.Controllers
{
    
    [ApiController]
    public class TasaUSController : ControllerBase
    {
        [HttpGet]
        [Route("apiv1/tasaus")]
        [AuthRequired]
        public IEnumerable<TasaUs> Get()
        {
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Carga",
                    Entidad = "TasaUS",
                    Fecha = DateTime.Now,
                    Metadata = null,
                    Pk = 0,
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            return new FactuMexContext().TasaUs.OrderByDescending(x=>x.Idtasa).Take(20);
        }

        [HttpPost]
        [Route("apiv1/tasaus")]
        [AuthRequired]
        public TasaUs Post(DTOTasa e)
        {
            var db = new FactuMexContext();
            if (e.Tasa.Idtasa == 0)
                db.TasaUs.Add(e.Tasa);
            else
                db.TasaUs.Attach(e.Tasa).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Tasa",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e.Tasa),
                    Pk = e.Tasa.Idtasa,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            return e.Tasa;
        }
    }
}