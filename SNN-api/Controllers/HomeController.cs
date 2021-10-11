using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace snn.Controllers
{
    [ApiController, Route("/")]
    public class HomeController : ControllerBase
    {
        IConfiguration myConfig;
        public HomeController(IConfiguration config)
        {
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
            else
            {
                appendWebmaster = "?";
            }
            myClient.Headers.Add("Authorization", $"Bearer {myConfig.GetValue<string>("Quotemedia:token")}");
            try
            {
                var myData = myClient.DownloadString($"http://app.quotemedia.com{HttpContext.Request.Path.ToString().Replace("qm/", "") + HttpContext.Request.QueryString}{appendWebmaster}webmasterId={myConfig.GetValue<string>("Quotemedia:webmasterid")}");
                return myData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("/filings")]
        public async Task<string> qmIndex(int index = 0, int size = 24)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"{myConfig.GetValue<string>("AppSettings:sec")}");
            try
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] buffer = encoding.GetBytes("{\"from\": \"" + index + "\",\"size\": \"" + size + "\",\"sort\": [{\"filedAt\": {\"order\": \"desc\"}}]}");
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                byteContent.Headers.ContentLength = buffer.Length;
                Task<string> response = client.PostAsync("https://api.sec-api.io", byteContent).Result.Content.ReadAsStringAsync();
                mydata mydata = System.Text.Json.JsonSerializer.Deserialize<mydata>(response.Result);
                string filingsString = System.Text.Json.JsonSerializer.Serialize(mydata.filings
                    .Select(f => new
                    {
                        ticker = f.ticker,
                        companyName = f.companyName,
                        formType = f.formType,
                        linkToTxt = f.linkToTxt,
                        linkToHtml = f.linkToHtml,
                        linkToFilingDetails = f.linkToFilingDetails,
                        filedAt = f.filedAt,
                        industry = f.industry
                    }));
                return filingsString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region Models
        class mydata
        {
            public object total { get; set; }
            public object query { get; set; }
            public List<filings> filings { get; set; }
        }
        class filings
        {
            public string ticker { get; set; }
            public string companyName { get; set; }
            public string formType { get; set; }
            public string linkToTxt { get; set; }
            public string linkToHtml { get; set; }
            public string linkToFilingDetails { get; set; }
            public string filedAt { get; set; }

            public string industry
            {
                get
                {
                    if (entities.Any() && !string.IsNullOrEmpty(entities.Where(e => e.sic != null).First().sic))
                    {
                        var indus = entities.Where(e => e.sic != null).First().sic.Split(" ").Skip(1);
                        return string.Join(" ", indus);
                    }
                    return "";
                }
            }
            public List<entities> entities { get; set; }
        }
        class entities
        {
            public string sic { get; set; }
        }
        #endregion
    }
}
