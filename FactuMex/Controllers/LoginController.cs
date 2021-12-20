using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CallCenter.Models;
namespace CallCenter.Controllers
{
    [Route("apiv1/iniciasesion")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //public Sesion Post(Ente e)
        //{
        //    CallCenterContext cc = new CallCenterContext();
        //    Ente u = cc.Ente.Where(nt => nt.Email == e.Email & nt.Pwd == e.Pwd).SingleOrDefault();
        //    Sesion ss = null;
        //    if (u != null)
        //    {
        //        ss = new Sesion { Desde = DateTime.Now, Hasta = DateTime.Now.AddHours(12), Idusuario = u.Idente, Token = Guid.NewGuid().ToString() };
        //        cc.Sesion.Add(ss);
        //        cc.SaveChanges();
        //    }
        //    return ss;
        //}
    }
}