using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace CallCenter.Models
{
    public class ServiceCall
    {
        private string _ServiceName { get; set; }
        private string _Params { get; set; }
        private bool EjecutaServicio()
        {
            var conf = new ConfigurationBuilder().AddJsonFile(".\\Config.json").Build();
            System.Diagnostics.Process Prc = new System.Diagnostics.Process { StartInfo = new System.Diagnostics.ProcessStartInfo() { FileName = _ServiceName , Arguments = _Params, WorkingDirectory = conf.GetValue<string>(_ServiceName), UseShellExecute = true } };
            Prc.Start();
            Prc.WaitForExit();
            return true;
        }
        public ServiceCall(string Servicio, string Params)
        {
            _ServiceName = Servicio;
            _Params = Params;
        }
        public bool Call()
        {
            return EjecutaServicio();
        }
    }
}
