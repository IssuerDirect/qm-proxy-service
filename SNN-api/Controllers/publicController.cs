
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
         myResponse.data=   lib.platformDB.snn_Insight.OrderByDescending(a=>a.id).Skip(pageSize * index).Take(pageSize).ToList();
            return myResponse;
        }

        [HttpPost("/public/loggedin")]
        public apiResponse loggedin(Dictionary<string,string> user)
        {
            if (!user.ContainsKey("email")) { return null; }
            var users = lib.platformDB.snn_users.Where(u => u.userid == user["userid"]).FirstOrDefault();
            if (users == null)
            {
                users = new snn_users();
                merge();
                lib.platformDB.snn_users.Add(users);
            }
            else {
                merge();
                lib.platformDB.snn_users.Update(users);
            }

            void merge() {
                users.email = user["email"];
                users.userid = user["userid"];
                if (user.ContainsKey("password")) {
                    users.password = clib.encrypt(user["password"]);
                }
            }

            string token = lib.GenerateJSONWebToken(users);
            myResponse.data = token;
            return myResponse;
        }
    }
}
