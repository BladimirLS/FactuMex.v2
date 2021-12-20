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
    public class EnteController : ControllerBase
    {
        [HttpGet]
        [Route("apiv1/ente")]
        [AuthRequired]
        public IEnumerable<Ente> Get()
        {
            string search = Request.Query.Where(v => v.Key == "s").SingleOrDefault().Value;
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Carga",
                    Entidad = "Cliente",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(search),
                    Pk = 0,
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            return new FactuMexContext().Ente.Where(e => e.Nombre.Contains(search) | e.Id.Contains(search));
        }

        [HttpGet]
        [Route("apiv1/ente/{id?}")]
        //[AuthRequired]
        public DTOEnte Get(string id)
        {
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Carga",
                    Entidad = "Cliente",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(id),
                    Pk = Convert.ToInt32(id),
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            var fm = new FactuMexContext();
            return new DTOEnte { Ente = fm.Ente.Where(e => e.Idente == Convert.ToInt32(id)).FirstOrDefault(), DirEntrega = fm.DirxEnte.Where(d => d.Idente == long.Parse(id) & d.TipoDir=="ent").LastOrDefault(), DirFacturacion= fm.DirxEnte.Where(d => d.Idente == long.Parse(id) & d.TipoDir == "fac").LastOrDefault() };
        }

        [HttpPost]
        [Route("apiv1/ente")]
        [AuthRequired]
        public Ente Post(DTOEnte e)
        {
            var db = new FactuMexContext();
            if (e.Ente.Idente == 0)
                db.Ente.Add(e.Ente);
            else
                db.Ente.Attach(e.Ente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Cliente",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e.Ente),
                    Pk = e.Ente.Idente,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });
            db.DirxEnte.RemoveRange(db.DirxEnte.Where(de => de.Idente == e.Ente.Idente));
            db.SaveChanges();
            e.DirEntrega.Idente = e.Ente.Idente;
            e.DirEntrega.Iddireccion = 0;
            e.DirFacturacion.Idente = e.Ente.Idente;
            e.DirFacturacion.Iddireccion = 0;
            db.DirxEnte.Add(e.DirEntrega);
            db.DirxEnte.Add(e.DirFacturacion);
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Direccion",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e.DirEntrega),
                    Pk = e.DirEntrega.Iddireccion,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Direccion",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e.DirFacturacion),
                    Pk = e.DirFacturacion.Iddireccion,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            return e.Ente;
        }



    }
}