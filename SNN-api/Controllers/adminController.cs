using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net3000;
using Microsoft.Extensions.Configuration;
using net3000.common;
using net3000.common.models;

namespace snn.Controllers
{
    [Route("/admin")]
    public class AdminController : Controller
    {
        apiResponse myResponse = new apiResponse();
        lib clib = new lib();
        SNNLib lib = new SNNLib();
        int pageSize = 24;
        public AdminController(IConfiguration config, platformDB platformDB)
        {
            lib.config = config;
            lib.platformDB = platformDB;
            clib.myConfiguration = config;
        }

        public IActionResult Index()
        {
            ViewData["msgBox"] = "null";
            return View();
        }

        [HttpPost("/login")]
        public apiResponse login([FromBody] Dictionary<string, string> credentials)
        {
            var user = new Dictionary<string, string>();
            user.Add("email", credentials["email"]);
            user.Add("password", credentials["password"]);
            myResponse = clib.logMeIn(user, HttpContext);
            if (myResponse.code != 200) { return myResponse; }
            if (credentials.ContainsKey("ReturnUrl"))
            {
                myResponse.data = credentials["ReturnUrl"];
            }
            myResponse.message = "You're logged in, I'll redirect you to your page";
            return myResponse;
        }
        /// <summary>
        /// Linked from admin area top menu
        /// </summary>
        /// <returns></returns>
        [HttpGet("/logout")]
        public IActionResult logout()
        {
            HttpContext.Response.Cookies.Delete("loginUser");
            if (Request.Query["messageid"] == "unauthorized")
            {
                myResponse = standardMessages.unauthorized;
                myResponse.message = "You're not authorized to access the page you were trying to access";
            }
            else
            {
                myResponse = standardMessages.saved;
                myResponse.title = "Logged Out";
                myResponse.message = "You're now logged out";
            }
            ViewData["currentView"] = "null";
            ViewData["msgBox"] = Newtonsoft.Json.JsonConvert.SerializeObject(myResponse);
            return View("index");
        }
        [HttpGet("/messageid/{messageID}")]
        public IActionResult messageid(string messageID)
        {
            if (messageID == "loginrequired")
            {
                myResponse = standardMessages.unauthorized;
                myResponse.message = "You need to login to access the page you were trying to access";
            }
            else
            {
                myResponse = clib.apiAppMessage(messageID);
            }
            ViewData["currentView"] = "null";
            ViewData["msgBox"] = Newtonsoft.Json.JsonConvert.SerializeObject(myResponse);
            return View("index");
        }
        bool isAdmin()
        {
            if (HttpContext.Request.Headers != null && HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                //Temporarily
                return HttpContext.Request.Headers["Authorization"] == "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9leHBpcmF0aW9uIjoiOS8xNi8yMDIxIDQ6MDY6NDEgQU0iLCJyb2xlIjoiMC0xIiwibmJmIjoxNjMxNzM2NDAxLCJleHAiOjE2MzE3Nzk2MDEsImlhdCI6MTYzMTczNjQwMX0.IWs7Dn-k0KaH08BQ4_FJKyUyVbx5nqkku0vNr6YqW1luVl0iuifGbWTht1rSdc9_KRU9x9TDY74kyPttZxj2vQ";
            }
            return false;
        }
    }
}
