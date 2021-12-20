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
    public class FacturaController : ControllerBase
    {
        [HttpGet]
        [Route("apiv1/factura")]
        [AuthRequired]
        public IEnumerable<DTOFac> Get()
        {
            string search = Request.Query.Where(v => v.Key == "s").SingleOrDefault().Value;
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            string idval = Request.Query.Where(v => v.Key == "idval").SingleOrDefault().Value;
            string val = Request.Query.Where(v => v.Key == "val").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Carga",
                    Entidad = "Factura",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(search),
                    Pk = 0,
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            var fm = new FactuMexContext();
            var lfac = new List<DTOFac>();
            if (idval == null)
                fm.Factura.ToList().ForEach(f =>
                {
                    var cl = fm.Ente.Where(e => e.Idente == f.Idcliente).FirstOrDefault();
                    var fac = new DTOFac { Rels = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Relacionado").ToList(), Tickets = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Ticket").ToList(), Referencia = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Referencia").SingleOrDefault(), Fac = f, Desglose = fm.DesgloseFactura.Where(d => d.Idfactura == f.Idfactura).ToList(), Cliente = new DTOEnte { Ente = cl, DirEntrega = fm.DirxEnte.Where(d => d.Idente == cl.Idente & d.TipoDir == "ent").LastOrDefault(), DirFacturacion = fm.DirxEnte.Where(d => d.Idente == cl.Idente & d.TipoDir == "fac").LastOrDefault() } };
                    lfac.Add(fac);
                });
            if (idval == "0")
            {
                var comps = fm.RefxFac.Where(r => r.TipoRef == "Relacionado").GroupBy(r => r.Idfac).Select(x => new { idfac = x.Key, conteo = x.Count() }).ToList();
                var incomps = fm.Incompleto.GroupBy(r => r.Idfac).Select(x => new { idfac = x.Key, conteo = x.Count() }).ToList();
                var dif = incomps.Where(i => comps.Where(c => c.idfac == i.idfac && c.conteo != i.conteo).Any());
                var incs = dif.Select(fx => fx.idfac).ToList();
                var facs = fm.FacxVal.Where(fval => !incs.Contains(fval.Idfac)).Select(fx => fx.Idfac).ToList();

                fm.Factura.Where(lf => !facs.Contains(lf.Idfactura)).ToList().ForEach(f =>
                  {
                      var cl = fm.Ente.Where(e => e.Idente == f.Idcliente).FirstOrDefault();
                      var inc = fm.Incompleto.Where(i => i.Idfac == f.Idfactura).ToList();
                      var rels = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Relacionado").ToList();
                      var relstr = rels.Select(r => r.Ref).Except(inc.Select(i => i.Relacionado)).ToList();
                      if (relstr.Count > 0 || rels.Count == 0)
                      {
                          string stat = "C";
                          if (inc.Count > 0)
                          {
                              rels = rels.Where(r => relstr.Contains(r.Ref)).ToList();
                              stat = "I";
                          }

                          var fac = new DTOFac { Stat = stat, Rels = rels, Tickets = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Ticket").ToList(), Referencia = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Referencia").SingleOrDefault(), Fac = f, Desglose = fm.DesgloseFactura.Where(d => d.Idfactura == f.Idfactura).ToList(), Cliente = new DTOEnte { Ente = cl, DirEntrega = fm.DirxEnte.Where(d => d.Idente == cl.Idente & d.TipoDir == "ent").LastOrDefault(), DirFacturacion = fm.DirxEnte.Where(d => d.Idente == cl.Idente & d.TipoDir == "fac").LastOrDefault() } };
                          lfac.Add(fac);
                      }
                  });
            }
            if (idval == "1")
            {
                IQueryable<long?> facs = fm.FacxDesVal.Select(fx => fx.Idfac);
                IQueryable<long?> facsVal = fm.FacxVal.Where(vf => vf.Idval == long.Parse(val)).Select(fx => fx.Idfac);
                var incomp = fm.Incompleto.Where(i => i.Recibido == null).Select(i => i.Idfac);
                fm.Factura.Where(lf => (facsVal.Contains(lf.Idfactura) && !facs.Contains(lf.Idfactura)) || incomp.Contains(lf.Idfactura)).ToList().ForEach(f =>
                {
                    var inc = fm.Incompleto.Where(i => i.Idfac == f.Idfactura && i.Recibido == null).Select(r => r.Relacionado).ToList();
                    string stat = "C";
                    List<RefxFac> refsInc = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Relacionado").ToList();
                    if ((refsInc.Count > inc.Count) && inc.Count > 0)
                        stat = "I";
                    else if ((refsInc.Count == inc.Count) && inc.Count > 0)
                        stat = "E";
                    if (inc.Count > 0)
                        refsInc = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Relacionado" & inc.Contains(r.Ref)).ToList();
                    var cl = fm.Ente.Where(e => e.Idente == f.Idcliente).FirstOrDefault();
                    var fac = new DTOFac { Stat = stat, Rels = refsInc, Tickets = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Ticket").ToList(), Referencia = fm.RefxFac.Where(r => r.Idfac == f.Idfactura & r.TipoRef == "Referencia").SingleOrDefault(), Fac = f, Desglose = fm.DesgloseFactura.Where(d => d.Idfactura == f.Idfactura).ToList(), Cliente = new DTOEnte { Ente = cl, DirEntrega = fm.DirxEnte.Where(d => d.Idente == cl.Idente & d.TipoDir == "ent").LastOrDefault(), DirFacturacion = fm.DirxEnte.Where(d => d.Idente == cl.Idente & d.TipoDir == "fac").LastOrDefault() } };
                    lfac.Add(fac);
                });
            }
            return lfac;
        }

        [HttpGet]
        [Route("apiv1/factura/{id?}")]
        [AuthRequired]
        public int Get(string id)
        {
            string usr = Request.Query.Where(v => v.Key == "usr").SingleOrDefault().Value;
            var db = new FactuMexContext();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Factura",
                    Entidad = "Cliente",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(id),
                    Pk = Convert.ToInt32(id),
                    Usuario = db.RenoUsuario.Where(u => u.Email == usr).SingleOrDefault().Idusuario
                });
            db.SaveChanges();
            var fm = new FactuMexContext();
            var f = fm.Factura.Where(fa => fa.Idfactura == long.Parse(id)).FirstOrDefault();
            var cl = fm.Ente.Where(e => e.Idente == f.Idcliente).FirstOrDefault();
            var fac = new DTOFac { Fac = f, Desglose = fm.DesgloseFactura.Where(d => d.Idfactura == f.Idfactura).ToList(), Cliente = new DTOEnte { Ente = cl, DirEntrega = fm.DirxEnte.Where(d => d.Idente == cl.Idente & d.TipoDir == "ent").LastOrDefault(), DirFacturacion = fm.DirxEnte.Where(d => d.Idente == cl.Idente & d.TipoDir == "fac").LastOrDefault() } };
            return 0;
        }
        [HttpPost]
        [Route("apiv1/factura")]
        [AuthRequired]
        public DTOFac Post(DTOFac e)
        {
            long sec = long.Parse(new DB(Cns.Renomax, "CreaNCF", new List<object>() { e.Fac.Idempresa }.ToArray()).EjecutaCmd().ToString());
            var db = new FactuMexContext();
            string NCF = db.Comprobante.Where(c => c.Idcomp == e.Fac.Idempresa).FirstOrDefault().EstatDesde;
            NCF = NCF.Remove(NCF.Length - sec.ToString().Length) + sec.ToString();
            e.Fac.Ncf = NCF;
            e.Fac.Fecha = DateTime.Now;

            if (e.Fac.Idfactura == 0)
                db.Factura.Add(e.Fac);
            else
                db.Factura.Attach(e.Fac).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            if (e.Fac.FormaPago == "Crédito")
                db.Abono.Add(new Abono { Idfactura = e.Fac.Idfactura, Pagado = e.Pagado });
            e.Desglose.ForEach(d => d.Idfactura = e.Fac.Idfactura);
            e.Tickets.ForEach(d => d.Idfac = e.Fac.Idfactura);
            e.Rels.ForEach(d => d.Idfac = e.Fac.Idfactura);
            e.Referencia.Idfac = e.Fac.Idfactura;
            db.DesgloseFactura.RemoveRange(db.DesgloseFactura.Where(df => df.Idfactura == e.Fac.Idfactura));
            db.RefxFac.RemoveRange(db.RefxFac.Where(df => df.Idfac == e.Fac.Idfactura));
            db.DesgloseFactura.AddRange(e.Desglose);
            db.RefxFac.AddRange(e.Tickets);
            db.RefxFac.AddRange(e.Rels);
            db.RefxFac.Add(e.Referencia);
            db.SaveChanges();

            new DB(Cns.Renomax, "ActualizaNCF", new List<object>() { e.Fac.Idempresa, sec.ToString(), NCF, e.Fac.Idfactura }.ToArray()).EjecutaTSQL();
            db.Bitacora.Add(
                new Bitacora
                {
                    Accion = "Guardar",
                    Entidad = "Factura",
                    Fecha = DateTime.Now,
                    Metadata = JsonConvert.SerializeObject(e),
                    Pk = e.Fac.Idfactura,
                    Usuario = db.RenoUsuario.Where(u => u.Email == Request.HttpContext.Request.Headers["usr"]).SingleOrDefault().Idusuario
                });

            db.SaveChanges();

            return e;
        }
    }
}