using System.IO;
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

namespace snn.Controllers
{
    [Route("/public")]
    public class publicController : Controller
    {
        apiResponse myResponse = new apiResponse();
        lib clib = new lib();
        SNNLib lib = new SNNLib();
        int pageSize = 24;
        public publicController(IConfiguration config, platformDB platformDB)
        {
            lib.config = config;
            lib.platformDB = platformDB;
            clib.myConfiguration = config;
        }

        [HttpGet("/public/insights")]
        public apiResponse insights(int index = 0)
        {
            myResponse = standardMessages.found;
            var myList = lib.platformDB.snn_Insight.Where(i => i.id > 86 && (i.ref_Status == 125 || i.ref_Status == 25)).Include(i => i.ref_StatusObject).Include(i => i.ref_InsightType)
                .Select(i => new snn_Insight()
                {
                    id = i.id,
                    body = clib.des(i.body, 125, "..."),
                    create_time = i.create_time,
                    image = i.image,
                    link = i.link,
                    ref_InsightType = i.ref_InsightType,
                    ref_Status = i.ref_Status,
                    ref_StatusObject = i.ref_StatusObject,
                    src = i.src,
                    title = i.title,
                    type = i.type,
                    update_time = i.update_time
                });
            myResponse.count = myList.Count();
            myResponse.data = myList.OrderByDescending(a => a.id).Skip(pageSize * index).Take(pageSize).ToList();
            return myResponse;
        }
        [HttpGet("/public/insight")]
        public apiResponse insight(int id)
        {
            if (id != 0)
            {
                myResponse = standardMessages.found;
                var model = lib.platformDB.snn_Insight.Where(i => i.id == id).Include(i => i.ref_StatusObject).Include(i => i.ref_InsightType).FirstOrDefault();
                if (model != null)
                {
                    myResponse.data = model;
                    return myResponse;
                }
            }
            myResponse = standardMessages.notFound;
            return myResponse;
        }
        [HttpPost("/public/loggedin")]
        public apiResponse loggedin(Dictionary<string, string> user)
        {
            if (!user.ContainsKey("email")) { return null; }
            var users = lib.platformDB.snn_users.Where(u => u.userid == user["userid"]).FirstOrDefault();
            if (users == null)
            {
                users = new snn_users();
                merge();
                lib.platformDB.snn_users.Add(users);
            }
            else
            {
                merge();
                lib.platformDB.snn_users.Update(users);
            }

            void merge()
            {
                users.email = user["email"];
                users.userid = user["userid"];
                if (user.ContainsKey("password"))
                {
                    users.password = clib.encrypt(user["password"]);
                }
                users.firstName = user["first_name"];
                users.lastname = user["last_name"];
                users.phone = user["phone"];
                users.ref_Country = user["ref_Country"];
                users.additional = user["additional"];
            }

            string token = lib.GenerateJSONWebToken(users);
            myResponse.data = token;
            return myResponse;
        }

        [HttpPost("/uploadImages")]
        public ckeditorResponse uploadImages([FromQuery] string guid = null, [FromQuery] int? id = null)
        {
            ckeditorResponse fileList = new ckeditorResponse();
            if (HttpContext.Request.Form.Files == null || HttpContext.Request.Form.Files.Count() == 0) { return new ckeditorResponse(); }
            foreach (var file in HttpContext.Request.Form.Files)
            {
                string myName = clib.GetFriendlyLink(Path.GetFileNameWithoutExtension(file.FileName)) + Path.GetExtension(file.FileName);
                var myStream = new MemoryStream();
                file.CopyTo(myStream);
                fileList.fileName = myName;
            }

            return fileList;
        }
    }
}
