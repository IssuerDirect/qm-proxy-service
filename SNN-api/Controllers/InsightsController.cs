using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using net3000;
using net3000.common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snn.Controllers
{
    [Route("/admin/Insights")]

    [AutoValidateAntiforgeryToken]
    public class InsightsController : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        int pageSize = 50;

        public InsightsController(IConfiguration configuration, platformDB snnDB)
        {
            lib.platformDB = snnDB;
            clib.myConfiguration = configuration;
        }
        [HttpGet("/admin/Insights")]
        public IActionResult Index(string keywords = null, int Type = 0, int status = -90, int pageIndex = 0)
        {
            //if (!readContext()) { return Unauthorized(); }
            myResponse = standardMessages.found;
            var insights = lib.platformDB.snn_Insight.Where(a => (Type == 0 || a.type == Type) && (status == -90 || a.ref_Status == status) && (keywords == null || a.title.Contains(keywords))).
                  Include(Z => Z.ref_InsightType).Include(a => a.ref_Statuses).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            myResponse.count = insights.Count();
            myResponse.data = insights;
            myResponse.pageSize = pageSize;
            myResponse.pageIndex = pageIndex;
            ViewData["insights"] = System.Text.Json.JsonSerializer.Serialize(myResponse); ;
            ViewBag.statuses = lib.platformDB.ref_Status.Select(a => new SelectListItem() { Value= a.id.ToString(), Text= a.name }).ToList();
            ViewBag.types = lib.platformDB.ref_InsightType.Select(a => new  SelectListItem() { Value=a.id.ToString(),  Text= a.name }).ToList();
            return View();
        }

        [HttpPost("/admin/insight")]
        public apiResponse saveInsight([FromBody] snn_Insight insight)
        {
            if (!readContext()) { return standardMessages.unauthorized; }
            lib.platformDB.snn_Insight.Add(insight);
            lib.platformDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = insight;
            return myResponse;
        }

        [HttpDelete("/admin/Insight/{ids}")]
        public apiResponse delete([FromQuery] string ids)
        {
            if (!readContext()) { return standardMessages.invalid; }
            var IDS = ids.Split(',').Select(a => Convert.ToInt32(a)).ToList<int>();
            var tobeRemoved = lib.platformDB.snn_Insight.Where(a => IDS.Contains(a.id)).ToList();
            if (tobeRemoved.Any())
            {
                lib.platformDB.snn_Insight.RemoveRange(tobeRemoved);
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
