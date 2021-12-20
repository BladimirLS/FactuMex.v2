using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace Renomax.Controllers
{
    
    [ApiController]
    public class ImgController : ControllerBase
    {
        [HttpGet]
        [Route("apiv1/descimg/{f?}")]
        public FileResult Get(string f)
        {
            var conf = new ConfigurationBuilder().AddJsonFile(".\\Config.json").Build();
            string Nombre = f;
            byte[] Contenido = null;
            try
            {
                Contenido = System.IO.File.ReadAllBytes(conf.GetValue<string>("Img") + "\\" + f);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Contenido = System.IO.File.ReadAllBytes(conf.GetValue<string>("Img") + "\\c.png");
                Contenido = new byte['1'];
            }
            return File(Contenido, "image/jpeg", Nombre);
        }
    }
}