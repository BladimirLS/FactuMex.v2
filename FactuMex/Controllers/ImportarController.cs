using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renomax.Models;
using Microsoft.Extensions.Configuration;
namespace Renomax.Controllers
{
    [Route("apiv1/importar")]
    [ApiController]
    public class ImportarController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public DataTable Get()
        {
            return new DB(Cns.Renomax, "CargaArchivos", null).RetornaDT();
        }
        [HttpPost]
        [AuthRequired]
        public long Post(ListArchivos archivos)
        {
            var conf = new ConfigurationBuilder().AddJsonFile(".\\Config.json").Build();
            string TimeStamp = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds().ToString();
            Parallel.ForEach(archivos.Archivos, a =>
            {
                string fName = conf.GetValue<string>("Import") + TimeStamp + "-" + a.Nombre;
                System.IO.File.WriteAllBytes(fName, Convert.FromBase64String(a.Base64Url));
                System.Diagnostics.Process Prc = new System.Diagnostics.Process { StartInfo = new System.Diagnostics.ProcessStartInfo() { FileName = $"Import.exe", Arguments = $"\"{fName}\" \"{a.Nombre}\"", WorkingDirectory = conf.GetValue<string>("Renobins"), UseShellExecute = true } };
                Prc.Start();
                Prc.WaitForExit();
            });
            return 0;
        }
    }
}