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
    [Route("apiv1/comprobante")]
    [ApiController]
    public class ComprobanteController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public IEnumerable<Comprobante> Get()
        {
            string search = Request.Query.Where(v => v.Key == "s").SingleOrDefault().Value;
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Carga",
                    Entidad = "Comprobante",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(search),
                    Pk = 0,
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            return new FactuMexContext().Comprobante;
        }
        [HttpPost]
        [AuthRequired]
        public Comprobante Post(DTOComp e)
        {
            var db = new FactuMexContext();
            if (e.comp.Idcomp== 0)
                db.Comprobante.Add(e.comp);
            else
                db.Comprobante.Attach(e.comp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Comprobante",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e.comp),
                    Pk = e.comp.Idcomp,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });

            db.SaveChanges();
            return e.comp;
        }
    }
}