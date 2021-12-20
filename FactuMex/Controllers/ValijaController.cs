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
    [Route("apiv1/valija")]
    [ApiController]
    public class ValijaController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public IEnumerable<Valija> Get()
        {
            string search = Request.Query.Where(v => v.Key == "s").SingleOrDefault().Value;
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Carga",
                    Entidad = "Valija",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(search),
                    Pk = 0,
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            return new FactuMexContext().Valija;
        }
        [HttpPost]
        [AuthRequired]
        public Valija Post(DTOVal e)
        {
            var db = new FactuMexContext();
            e.val.Fecha = DateTime.Now;
            if (e.val.Idvalija == 0)
                db.Valija.Add(e.val);
            else
                db.Valija.Attach(e.val).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Valija",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e.val),
                    Pk = e.val.Idvalija,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });

            db.SaveChanges();
            return e.val;
        }
    }
}