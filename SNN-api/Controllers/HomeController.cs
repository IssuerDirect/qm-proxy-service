using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.IO;
using net3000;

namespace snn.Controllers
{
    [ApiController, Route("/")]
    public class HomeController : ControllerBase
    {
        IConfiguration myConfig;
        apiResponse myResponse = standardMessages.found;
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
        public async Task<apiResponse> qmIndex(int index = 0, int size = 24)
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
                var response = client.PostAsync("https://api.sec-api.io", byteContent);
                var rawResponse = await response.Result.Content.ReadAsStringAsync();
                fillingObject apiObject = System.Text.Json.JsonSerializer.Deserialize<fillingObject>(rawResponse);
                myResponse.data = apiObject.filings.Select(f => new
                {
                    f.companyName,
                    f.filedAt,
                    f.ticker,
                    f.linkToHtml,
                    f.linkToTxt,
                    f.linkToXbrl,
                    f.linkToFilingDetails,
                    f.industry,
                    f.formType
                });
            }
            catch (Exception ex)
            {
                myResponse = standardMessages.invalid;
                myResponse.message = ex.Message;
            }
            return myResponse;
        }
    }    
}
