using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace snn.Controllers
{
    [ApiController, Route("/")]
    public class HomeController : ControllerBase
    {
        IConfiguration myConfig;
        public HomeController(IConfiguration config) {
            myConfig = config;
        }

        public IActionResult Index()
        {
            return Content("SNN Proxy API");
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

        [HttpGet("/filings")]
        public string qmIndex(int index = 0, int size = 24)
        {
            WebClient myClient = new WebClient();
            myClient.Headers.Add("Authorization", $"{myConfig.GetValue<string>("AppSettings:sec")}");
            try
            {
                //var myData = myClient.DownloadString($"http://app.quotemedia.com{HttpContext.Request.Path.ToString().Replace("qm/", "") + HttpContext.Request.QueryString}{appendWebmaster}webmasterId={myConfig.GetValue<string>("Quotemedia:webmasterid")}");
                //return myData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    
}
