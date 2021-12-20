using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renomax.Models;

namespace Renomax.Controllers
{
    [Route("apiv1/masiva")]
    [ApiController]
    public class MasivoController : ControllerBase
    {
        [HttpPost]
        [AuthRequired]
        public int Post(DTOCampos Campos)
        {
            if (Campos.TipoActualizacion == "Asignacion")
            {
                var dt = new DB(Cns.Renomax, "CargaAsignacionesTODAS", new List<object>() { Campos }.ToArray()).RetornaDT();
                int f = dt.Rows.Count - 1;
                int colCedula = 0, colValor = 1;
                if (dt.Columns[0].ColumnName != "NroFiscal")
                {
                    colCedula = 1; colValor = 0;
                }
                for (int i = 0; i <= f; i++)
                {
                    string Cedula = dt.Rows[i].ItemArray[colCedula].ToString().PadLeft(11, '0');
                    if (float.TryParse(dt.Rows[i].ItemArray[colValor].ToString(), out float Valor))
                        new DB(Cns.Inb, "CargaAsignaciones", new List<object>() { Cedula, Valor }.ToArray()).EjecutaCmd();
                }
            }
            else
            {
                new DB(Cns.Renomax, "ActualizaDatosTODAS", new List<object>() { Campos }.ToArray()).EjecutaCmd();
            }
            return 0;
        }
    }
}