﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using net3000;
using Microsoft.Extensions.Configuration;
using net3000.common;
using net3000.common.models;

namespace snn.Controllers
{
    [Route("/"), AutoValidateAntiforgeryToken, ApiExplorerSettings(GroupName = "Admin Actions")]
    public class AdminController : Controller
    {
        apiResponse myResponse = new apiResponse();
        lib clib = new lib();
        SNNLib lib = new SNNLib();

        public AdminController(IConfiguration config, companyHubDB companyHubDB, peopleHubDB peopleHubDB)
        {
            lib.config = config;
            lib.companyHubDB = companyHubDB;
            lib.peopleHubDB = peopleHubDB;
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
            credentials.Add("logingroupid", "0-1");
            myResponse = lib.logMeIn(credentials, HttpContext);
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

        [HttpGet("/admin/companies")]
        public apiResponse companies()
        {
            var cc = lib.companyHubDB.ci_Company.Select(a => new { a.id, a.name }).OrderBy(a => a.name).ToList();
            myResponse = standardMessages.found;
            myResponse.data = cc;
            myResponse.count = cc.Count();
            return myResponse;
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
        
    }
}