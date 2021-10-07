﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.IO;

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
        public async Task<string> qmIndex(int index = 0, int size = 24)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"{myConfig.GetValue<string>("AppSettings:sec")}");
            try
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] buffer = encoding.GetBytes("{\"from\": \"" + index + "\",\"size\": \"" + size + "\",\"sort\": [{\"filedAt\": {\"order\": \"desc\"}}]}");
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("text/json");
                byteContent.Headers.ContentLength = buffer.Length;
                var response = client.PostAsync("https://api.sec-api.io", byteContent);
                return await response.Result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }    
}
