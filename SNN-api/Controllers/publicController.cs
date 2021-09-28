﻿using System.IO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net3000;
using Microsoft.Extensions.Configuration;
using net3000.common;
using net3000.common.models;
using Microsoft.EntityFrameworkCore;
using static snn.Models.publicModels;
using net3000.accounts.dbContext;

namespace snn.Controllers
{
    [Route("/public")]
    public class publicController : Controller
    {
        apiResponse myResponse = new apiResponse();
        lib clib = new lib();
        SNNLib lib = new SNNLib();
        emailMessage emsg = new emailMessage();
        int pageSize = 24;
        public publicController(IConfiguration config, peopleHubDB peopleHubDB, companyHubDB companyHubDB)
        {
            lib.config = config;
            lib.companyHubDB = companyHubDB;
            lib.peopleHubDB = peopleHubDB; 
            clib.myConfiguration = config;
            emsg.configuration = config;

        }

        /// <summary>
        /// Post Issue to admin.
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        /// <remarks>Post an issue with all details selected in the box. This will send an email to admin.</remarks>
        [HttpPost("/public/issue"), ApiExplorerSettings(GroupName = "Public Actions")]
        public IActionResult reportIssue(reportIssues issue)
        {
                emsg.setEmailTemplate = "report_issue";
                emsg.emsg.Body = clib.replacePlaceholders(emsg.emsg.Body, issue);
                emsg.subject = clib.replacePlaceholders(emsg.subject,issue);
            if (clib.testMode)
            {
                emsg.sendTo.Add("info@net3000.ca");
            }
            else
            {
                emsg.sendTo.Add(clib.webmaster());
            }
            emsg.send();
            return Ok();
        }


        /// <summary>
        /// View Latest Video
        /// </summary>
        /// <returns></returns>
        /// <remarks>View latest video posted</remarks>
        [HttpGet("/public/latestvideo"), ApiExplorerSettings(GroupName = "Public Actions")]
        public apiResponse latestvideo()
        { 
            var video = lib.companyHubDB.cc_SnnVideos.OrderByDescending(v => v.create_time).FirstOrDefault();
            if (video != null)
            {
                myResponse = standardMessages.found;
                myResponse.data = video;
                return myResponse;
            }
            myResponse = standardMessages.notFound;
            return myResponse;
        }

        //[HttpPost("/public/loggedin")]
        //public apiResponse loggedin(Dictionary<string, string> user)
        //{
        //    if (!user.ContainsKey("email")) { return null; }
        //    var users = lib.peopleHubDB.loginUsers.Where(u => u.userid == user["userid"]).FirstOrDefault();
        //    if (users == null)
        //    {
        //        users = new loginUsers();
        //        merge();
        //        lib.peopleHubDB.loginUsers.Add(users);
        //    }
        //    else
        //    {
        //        merge();
        //        lib.peopleHubDB.loginUsers.Update(users);
        //    }

        //    void merge()
        //    {
        //        users.email = user["email"];
        //        users.userid = user["userid"];
        //        if (user.ContainsKey("password"))
        //        {
        //            users.password = clib.encrypt(user["password"]);
        //        }
        //        users.firstName = user["first_name"];
        //        users.lastname = user["last_name"];
        //        users.phone = user["phone"];
        //    }

        //    string token = lib.GenerateJSONWebToken(users);
        //    myResponse.data = token;
        //    return myResponse;
        //}

        //[HttpPost("/uploadImages")]
        //public ckeditorResponse uploadImages([FromQuery] string guid = null, [FromQuery] int? id = null)
        //{
        //    ckeditorResponse fileList = new ckeditorResponse();
        //    if (HttpContext.Request.Form.Files == null || HttpContext.Request.Form.Files.Count() == 0) { return new ckeditorResponse(); }
        //    foreach (var file in HttpContext.Request.Form.Files)
        //    {
        //        string myName = clib.GetFriendlyLink(Path.GetFileNameWithoutExtension(file.FileName)) + Path.GetExtension(file.FileName);
        //        var myStream = new MemoryStream();
        //        file.CopyTo(myStream);
        //        fileList.fileName = myName;
        //    }

        //    return fileList;
        //}
    }
}
