using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using CallCenter.Models;
using Microsoft.AspNetCore.Http.Internal;

namespace Renomax.Models
{
    public class AuthRequired : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool Validado = false;
            switch (context.HttpContext.Request.Method.ToLower())
            {
                case "get":
                    Validado = Convert.ToBoolean(new DB(Cns.Renomax, "ValidaToken", new List<object>() { context.HttpContext.Request.Query["usr"], context.HttpContext.Request.Query["token"] }.ToArray()).EjecutaCmd());
                    break;
                case "post":
                    
                    //var req = context.HttpContext.Request;
                    
                    //req.EnableRewind();
                    
                        context.HttpContext.Request.Body.Position = 0;

                        JObject Body = JsonConvert.DeserializeObject<dynamic>(new StreamReader(context.HttpContext.Request.Body).ReadToEnd());
                        string Token = Body["token"].Value<string>();
                        string Usuario = Body["usr"].Value<string>();
                        Body.Remove("token");
                        Body.Remove("usr");
                        context.HttpContext.Request.Headers.Add("usr", new Microsoft.Extensions.Primitives.StringValues(Usuario));
                        context.HttpContext.Request.Body = GenerateStreamFromString(Body.ToString());
                        Validado = Convert.ToBoolean(new DB(Cns.Renomax, "ValidaToken",
                            new List<object>() {Usuario, Token}.ToArray()).EjecutaCmd());
                        break;

                    //req.Body.Position = 0;

            }
            if (Validado)
                base.OnActionExecuting(context);
            else
                context.Result = new JsonResult(new { denied = true });

        }
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

}
