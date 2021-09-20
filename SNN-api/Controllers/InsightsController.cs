using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using net3000;
using net3000.common.models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snn.Controllers
{
    [Route("/admin/Insights"), Authorize]
    public class InsightsController : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        int pageSize = 24;

        public InsightsController(IConfiguration configuration, platformDB snnDB)
        {
            lib.platformDB = snnDB;
            clib.myConfiguration = configuration;
        }

        [HttpGet("/admin/Insights")]
        public IActionResult Index(string keywords = null, int? type = null, int? status = null, int pageIndex = 0, bool json = false)
        {
            if (!readContext()) { return Unauthorized(); }
            myResponse = standardMessages.found;
            IQueryable<snn_Insight> insightQuery = lib.platformDB.snn_Insight.Where(i => i.id > 86).Include(Z => Z.ref_InsightType).Include(a => a.ref_StatusObject);
            if (type.HasValue) {
                insightQuery = insightQuery.Where(a => a.type == type.Value);
            }
            if (status.HasValue)
            {
                insightQuery = insightQuery.Where(a => a.ref_Status == status.Value);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                insightQuery = insightQuery.Where(a => a.title.ToLower().Contains(keywords.ToLower()));
            }
            myResponse.count = insightQuery.Count();
            myResponse.data = insightQuery.OrderByDescending(a => a.id).ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList(); ;
            myResponse.pageSize = pageSize;
            myResponse.pageIndex = pageIndex;
            if (json)
            {
                return Json(myResponse);
            }
            ViewData["insights"] = System.Text.Json.JsonSerializer.Serialize(myResponse);
            fillDataBags();
            return View();
        }

        void fillDataBags(snn_Insight insight = null) {
            if (insight != null) {
                if (insight.id == 0)
                {
                    ViewData["title"] = "Create Insight";
                }
                else
                {
                    ViewData["title"] = "Edit Insight# " + insight.id;
                }
            }
            ViewBag.statuses = lib.platformDB.ref_Status.Select(a => new SelectListItem() { Value = a.id.ToString(), Text = a.name }).ToList();
            ViewBag.types = lib.platformDB.ref_InsightType.Select(a => new SelectListItem() { Value = a.id.ToString(), Text = a.name }).ToList();
        }

        [HttpGet("/admin/Insights/details/{id?}")]
        public IActionResult details(int? id = null)
        {
            if (!readContext()) { return Unauthorized(); }
            snn_Insight model = new snn_Insight();
            if (id.HasValue)
            {
                model = lib.platformDB.snn_Insight.Where(i => i.id == id).FirstOrDefault();
                if (model == null)
                {
                    return NotFound();
                }
            }
            fillDataBags(model);
            return View("details", model);
        }

        [HttpPost("/admin/Insights/details")]
        public IActionResult saveInsight(snn_Insight insight)
        {
            if (!readContext()) { return Unauthorized(); }
            var myInsight = new snn_Insight();
            if (insight.id == 0)
            {
                lib.platformDB.snn_Insight.Add(insight);
            }
            else
            {
                myInsight = lib.platformDB.snn_Insight.Where(i => i.id == insight.id).FirstOrDefault();
                if (myInsight == null) { return NotFound(); }
                clib.mergeChanges(myInsight, insight);
                lib.platformDB.snn_Insight.Update(myInsight);
            }
            lib.platformDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = insight;
            fillDataBags(insight);
            TempData["msgBox"] = myResponse.html;
            return RedirectToAction("details", new { id = insight.id});
        }

        [HttpDelete("/admin/Insight")]
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
            lib.myUser(User);
            if (lib.user.id <= 0) { return false; }
            return true;
        }
    }
}
