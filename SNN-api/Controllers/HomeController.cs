using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;
using net3000;

namespace snn.Controllers
{
    [ApiController, ApiExplorerSettings(GroupName = "Quote Media")]
    public class HomeController : ControllerBase
    {
        IConfiguration myConfig;
        public HomeController(IConfiguration config) {
            myConfig = config;
        }

        [HttpGet("qm/{*queryvalues}")]
        public string qmIndex()
        {
            WebClient myClient = new WebClient();
            string appendWebmaster = null;
            if (HttpContext.Request.Query != null && HttpContext.Request.Query.Keys.Count > 0)
            {
                appendWebmaster = "&";
            }
            else {
                appendWebmaster = "?";
            }
            myClient.Headers.Add("Authorization", $"Bearer {myConfig.GetValue<string>("Quotemedia:token")}");
            try {
                var myData = myClient.DownloadString($"http://app.quotemedia.com{HttpContext.Request.Path.ToString().Replace("qm/", "") + HttpContext.Request.QueryString}{appendWebmaster}webmasterId={myConfig.GetValue<string>("Quotemedia:webmasterid")}");
                return myData;
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }
    }
    
}
