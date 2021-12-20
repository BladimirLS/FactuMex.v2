using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renomax.Models;
using Microsoft.Extensions.Configuration;
namespace Renomax.Controllers
{
    [Route("apiv1/tabla")]
    [ApiController]
    public class TablaController : ControllerBase
    {
        [HttpGet]
        [AuthRequired]
        public dynamic Get()
        {
            var conf = new ConfigurationBuilder().AddJsonFile(".\\Config.json").Build();
            string tab = Request.Query.Where(t => t.Key == "t").SingleOrDefault().Value;
            SqlConnection cn = new SqlConnection(conf.GetValue<string>("Renomax"));
            cn.Open();
            string q = $@"SELECT TOP 5 * FROM dbo.[{tab}]";

            SqlCommand cmdBus = new SqlCommand(q, cn);
            SqlDataReader r = cmdBus.ExecuteReader();
            int cols = r.FieldCount;
            List<GroupResult> gr = new List<GroupResult>();
            object Tracking = null;
            while (r.Read())
            {
                List<SearchResult> sr = new List<SearchResult>();
                SearchResult res = null;

                for (int col = 0; col < cols; col++)
                {
                    res = new SearchResult
                    {
                        Label = '['+r.GetName(col)+']',
                        Value = r.GetValue(col)
                    };
                    if (res.Label == "Tracking")
                        Tracking = res.Value;
                    sr.Add(res);
                }
                gr.Add(new GroupResult { Result = sr.ToArray(), Tracking = Tracking });
            }
            cn.Close();
            return gr;
        }
    }
}