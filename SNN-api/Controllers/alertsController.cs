using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using net3000;
using Microsoft.AspNetCore.Authorization;
using net3000.common.models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace snn.Controllers
{
    [Route("/admin/alerts"), AutoValidateAntiforgeryToken, Authorize]
    public class alertsController : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        int pageSize = 50;

        public alertsController(IConfiguration configuration, platformDB snnDB)
        {
            lib.platformDB = snnDB;
            clib.myConfiguration = configuration;
        }

        public IActionResult Index(string keywords = null, int pageIndex = 0)
        {
            if (!readContext()) { return Unauthorized(); }
            myResponse = standardMessages.found;
            var alerts = lib.platformDB.snn_alerts.Where(a => (keywords == null || a.details.Contains(keywords))).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            myResponse.count = alerts.Count();
            myResponse.data = alerts;
            myResponse.pageSize = pageSize;
            myResponse.pageIndex = pageIndex;
            ViewData["alerts"] = System.Text.Json.JsonSerializer.Serialize(myResponse);
            return View();
        }

        [HttpPost("/admin/alert")]
        public apiResponse saveInsight([FromBody] snn_alerts alert)
        {
            if (!readContext()) { return standardMessages.unauthorized; }
            lib.platformDB.snn_alerts.Add(alert);
            lib.platformDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = alert;
            return myResponse;
        }

        [HttpDelete("/admin/alert/{ids}")]
        public apiResponse delete([FromQuery] string ids)
        {
            if (!readContext()) { return standardMessages.invalid; }
            var IDS = ids.Split(',').Select(a => Convert.ToInt32(a)).ToList<int>();
            var tobeRemoved = lib.platformDB.snn_alerts.Where(a => IDS.Contains(a.id)).ToList();
            if (tobeRemoved.Any())
            {
                lib.platformDB.snn_alerts.RemoveRange(tobeRemoved);
                lib.platformDB.SaveChanges();
                myResponse = standardMessages.deleted;
                myResponse.data = ids;
            }
            else
            {
                myResponse = standardMessages.notFound;
            }
            return myResponse;
        }

        bool readContext()
        {
            //clib.myUser(User);
            //if (clib.account <= 0) { return false; }
            return true;
        }
    }
}
