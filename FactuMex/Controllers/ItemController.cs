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

    [ApiController]
    public class ItemController : ControllerBase
    {
        [HttpGet]
        [Route("apiv1/item")]
        [AuthRequired]
        public IEnumerable<Item> Get()
        {
            string search = Request.Query.Where(v => v.Key == "s").SingleOrDefault().Value;
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Carga",
                    Entidad = "Item",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(search),
                    Pk = 0,
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            if (search == "(TODOS)")
                return new FactuMexContext().Item.ToList();
            else
                return new FactuMexContext().Item.Where(e => e.Nombre.Contains(search) | e.Descripcion.Contains(search));
        }
        [HttpGet]
        [Route("apiv1/item/{id?}")]
        [AuthRequired]
        public DTOItem Get(string id)
        {
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Carga",
                    Entidad = "Item",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(id),
                    Pk = long.Parse(id),
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            var fm = new FactuMexContext();
            return new DTOItem { Item = fm.Item.Where(e => e.Iditem == long.Parse(id)).FirstOrDefault(), PrecioItem = fm.PrecioxItem.Where(p => p.Iditem == long.Parse(id)).ToList(), SLAItem = fm.SlaxItem.Where(p => p.Iditem == long.Parse(id)).ToList() };
        }
        [HttpPost]
        [Route("apiv1/item")]
        [AuthRequired]
        public Item Post(DTOItem e)
        {
            var db = new FactuMexContext();
            if (e.Item.Iditem == 0)
                db.Item.Add(e.Item);
            else
                db.Item.Attach(e.Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Item",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e.Item),
                    Pk = e.Item.Iditem,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });

            e.SLAItem.ToList().ForEach(s => { s.Iditem = e.Item.Iditem; });
            e.PrecioItem.ToList().ForEach(s => { s.Iditem = e.Item.Iditem; });
            db.SlaxItem.AddRange(e.SLAItem);
            db.PrecioxItem.AddRange(e.PrecioItem);
            db.SaveChanges();
            e.SLAItem.ToList().ForEach(s =>
            {
                db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "SLA",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(s),
                    Pk = s.Idsla,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });
            });
            e.PrecioItem.ToList().ForEach(p =>
            {
                db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Precio",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(p),
                    Pk = p.Idprecio,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });
            });
            db.SaveChanges();
            return e.Item;
        }
    }
}