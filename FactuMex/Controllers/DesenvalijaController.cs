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
    [Route("apiv1/desenvalija")]
    [ApiController]
    public class DesenvalijaController : ControllerBase
    {
        [HttpPost]
        [AuthRequired]
        public DTOValxFac Post(DTOValxFac e)
        {
            var db = new FactuMexContext();
            var fv = new FacxDesVal { Idfac = e.fac, Idval = e.val,Fec=DateTime.Now };
            var inc = db.Incompleto.Where(i => i.Idfac == e.fac).ToList();
            if (inc.Count > 0)
            {
                inc.ForEach(i => {
                    i.Recibido = true;
                    i.FecRec = DateTime.Now;
                    db.Incompleto.Attach(i).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
                }) ;
            }
            if (e.actn == "Guardar")
                db.FacxDesVal.Add(fv);
            else if (e.actn == "Borrar")
            {
                fv = db.FacxDesVal.Where(s => s.Idval == e.val && s.Idfac == e.fac).FirstOrDefault();
                db.FacxDesVal.Remove(fv);
            }
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = e.actn,
                    Entidad = "FacturaxDesenvalija",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e),
                    Pk = fv.IdfacxVal,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });

            db.SaveChanges();
            return e;
        }
    }
}