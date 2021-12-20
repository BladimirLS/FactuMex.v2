using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Renomax.Models;

namespace Renomax.Controllers
{
    [Route("apiv1/archivo")]
    [ApiController]

    public class ArchivoController : ControllerBase
    {
        public dynamic Post(Archivo archivo)
        {
            var conf = new ConfigurationBuilder().AddJsonFile(".\\Config.json").Build();
            string TimeStamp = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds().ToString();
            string Ano = new DB(Cns.Renomax, "GetAnoActual", new List<object>() { }.ToArray()).EjecutaCmd().ToString();
            string Validacion = new DB(Cns.Inb, "ValidaRenoExisteParametrizada", new List<object>() { archivo.Nombre, Ano }.ToArray()).EjecutaCmd().ToString();
            if (Validacion.Contains("TODOBIEN"))
            {
                string fName = conf.GetValue<string>("Import") + TimeStamp + "-" + archivo.Nombre.Replace(" ","_");
                System.IO.File.WriteAllBytes(fName, Convert.FromBase64String(archivo.Base64Url));
                System.Diagnostics.Process Prc = new System.Diagnostics.Process { StartInfo = new System.Diagnostics.ProcessStartInfo() { FileName = $"AgregaSolicitud.exe", Arguments = $"{fName}", WorkingDirectory = conf.GetValue<string>("AgregaSolicitud"), UseShellExecute = true } };
                Prc.Start();
                Prc.WaitForExit();
            }
            return new { validacion = Validacion };

        }

    }
}