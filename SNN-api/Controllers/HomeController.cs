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
using System.Collections.Generic;

namespace snn.Controllers
{
    [ApiController, Route("/")]
    public class HomeController : ControllerBase
    {
        IConfiguration myConfig;
        apiResponse myResponse = standardMessages.found;
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
        public async Task<apiResponse> qmIndex(int index = 0, int size = 24, string industry = null, DateTime? startdate = null, DateTime? enddate = null)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"{myConfig.GetValue<string>("AppSettings:sec")}");
            try
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                Root root = new Root();
                Sort sort = new Sort();
                root.from = (index * size).ToString();
                root.size = size.ToString();
                sort.filedAt.order = "desc";
                root.sort.Add(sort);
                string myParameters;
                if (!string.IsNullOrEmpty(industry))
                {
                    root.query.query_string.query = "entities.sic: \"" + industry + "\"";
                    if (startdate != null && enddate != null)
                    {
                        root.query.query_string.query += " AND filedAt:{" + startdate.Value.ToString("yyyy-MM-dd") + " TO " + enddate.Value.ToString("yyyy-MM-dd") + "}";
                    }
                    myParameters = System.Text.Json.JsonSerializer.Serialize(new { root.query, root.from, root.size, root.sort });
                }
                else if (startdate != null && enddate != null)
                {
                    root.query.query_string.query = "filedAt:{" + startdate.Value.ToString("yyyy-MM-dd") + " TO " + enddate.Value.ToString("yyyy-MM-dd") + "}";
                    myParameters = System.Text.Json.JsonSerializer.Serialize(new { root.query, root.from, root.size, root.sort });
                }
                else
                {
                    myParameters = System.Text.Json.JsonSerializer.Serialize(new { root.from, root.size, root.sort });
                }
                byte[] buffer = encoding.GetBytes(myParameters);
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
                    f.industry
                });
                myResponse.count = apiObject.total.value;
            }
            catch (Exception ex)
            {
                myResponse = standardMessages.invalid;
                myResponse.message = ex.Message;
            }
            return myResponse;
        }
        class Root
        {
            public Query query { get; set; } = new Query();
            public string from { get; set; }
            public string size { get; set; }
            public List<Sort> sort { get; set; } = new List<Sort>();
        }
        class QueryString
        {
            public string query { get; set; }
        }
        class Query
        {
            public QueryString query_string { get; set; } = new QueryString();
        }
        class FiledAt
        {
            public string order { get; set; } = "";
        }
        class Sort
        {
            public FiledAt filedAt { get; set; } = new FiledAt();
        }
    }
}
