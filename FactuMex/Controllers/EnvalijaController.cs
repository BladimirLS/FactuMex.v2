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
    public class EnvalijaController : ControllerBase
    {
        [Route("apiv1/envalija")]
        [HttpPost]
        [AuthRequired]
        public DTOValxFac Post(DTOValxFac e)
        {
            var db = new FactuMexContext();
            var fv = new FacxVal { Idfac = e.fac, Idval = e.val,Fec=DateTime.Now };
            var inc = db.Incompleto.Where(i => i.Idfac == e.fac).ToList();
            var incAr = inc.Select(x => x.Relacionado);
            if (e.actn == "Guardar")
            {
                db.FacxVal.Add(fv);
                if (inc.Count>0)
                {
                    var incs = new List<Incompleto>();
                    db.RefxFac.Where(r => r.TipoRef == "Relacionado" & r.Idfac == e.fac & !incAr.Contains(r.Ref)).ToList().ForEach(re=> {
                        incs.Add(new Incompleto {Despachado=true,Idfac=e.fac,Idval=e.val,Relacionado=re.Ref });
                    });
                    db.Incompleto.AddRange(incs);
                }
            }
                
            else if (e.actn == "Borrar")
            {
                fv = db.FacxVal.Where(s => s.Idval == e.val && s.Idfac == e.fac).FirstOrDefault();
                db.FacxVal.Remove(fv);
            }
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = e.actn,
                    Entidad = "FacturaxValija",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e),
                    Pk = fv.IdfacxVal,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });

            db.SaveChanges();
            return e;
        }
        [Route("apiv1/envalija2")]
        [HttpPost]
        [AuthRequired]
        public DTOValxFacInc Post(DTOValxFacInc e)
        {
            var db = new FactuMexContext();
            var fv = new FacxVal { Idfac = e.fac, Idval = e.val, Fec = DateTime.Now };
            List<Incompleto> i = new List<Incompleto>();
            e.rel.ForEach(r => {
                i.Add(new Incompleto {Despachado=true,Idfac=r.Idfac,Idval=e.val,Relacionado=r.Ref,FecDsp=DateTime.Now });
            });
            if (e.actn == "Guardar")
            { 
                db.FacxVal.Add(fv);
                db.Incompleto.AddRange(i);
            }
            else if (e.actn == "Borrar")
            {
                fv = db.FacxVal.Where(s => s.Idval == e.val && s.Idfac == e.fac).FirstOrDefault();
                db.FacxVal.Remove(fv);
            }
            db.SaveChanges();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = e.actn,
                    Entidad = "FacturaxValijaInc",
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