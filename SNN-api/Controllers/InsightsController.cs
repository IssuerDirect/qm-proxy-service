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
    [Route("/admin/Insights")]
    public class InsightsController : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        int pageSize = 20;

        public InsightsController(IConfiguration configuration,  companyHubDB snnDB,platformDB _platformDB)
        {
            lib.companyHubDB  = snnDB;
            lib.platformDB = _platformDB;
            clib.myConfiguration = configuration;
        }
       
        [HttpGet("/admin/Insights")]
        public IActionResult Index(string keywords = null, int? type = null,  int pageIndex = 0, bool json = false)
        {
            if (!readContext()) { return Unauthorized(); }
            myResponse = standardMessages.found;
            IQueryable<cc_SnnInsight> insightQuery = lib.companyHubDB.cc_SnnInsight;
            if (type.HasValue)
            {
                insightQuery = insightQuery.Where(a => a.ref_InsightType == type.Value);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                insightQuery = insightQuery.Where(a => a.title.ToLower().Contains(keywords.ToLower()));
            }
            myResponse.count = insightQuery.Count();
            myResponse.data = insightQuery.OrderByDescending(a => a.create_time).ThenByDescending(a=>a.id ).Include(a => a.ref_InsightTypeObject).Skip(pageSize * pageIndex).Take(pageSize).ToList(); ;
            myResponse.pageSize = pageSize;
            myResponse.pageIndex = pageIndex;
            if (json)
            {
                return Json(myResponse);
            }
            ViewData["insights"] = System.Text.Json.JsonSerializer.Serialize(myResponse);
            fillDataBags();
            ViewData["title"] = "SNN Insights";
            return View();
        }

        void fillDataBags(cc_SnnInsight insightID = null)
        {
            if (insightID != null)
            {
                ViewData["title"] = "Edit Insight: " + insightID.title;
            }
            else {
                ViewData["title"] = "Create Insight";
            }
                           
           // ViewBag.statuses = lib..ref_Status.Select(a => new SelectListItem() { Value = a.id.ToString(), Text = a.name }).ToList();
            ViewBag.types = lib.companyHubDB.ref_InsightType.Select(a => new SelectListItem() { Value = a.id.ToString(), Text = a.name }).ToList();
        }

        [HttpGet("/admin/Insights/details/{id?}")]
        public IActionResult details(string id = null)
        {
            if (!readContext()) { return Unauthorized(); }
            cc_SnnInsight model = new cc_SnnInsight();
            if (id != null)
            {
                model = lib.companyHubDB.cc_SnnInsight.Where(i => i.id == id).FirstOrDefault();
                if (model == null)
                {
                    return NotFound();
                }
                fillDataBags(model);
            }
            else {
                fillDataBags();
            }
            return View("details", model);
        }

        [HttpPost("/admin/Insights/import")]
        public IActionResult importInsight([FromQuery] string url, [FromQuery] int  typeID)
        {
            if (!readContext()) { return NotFound(standardMessages.unauthorized); }
            var importedList = lib.readFeed(url, typeID);
            myResponse = standardMessages.saved;
            myResponse.data = importedList;
            myResponse.count = importedList.Count;
            return Json(myResponse);
        }

        [HttpPost("/admin/Insights/details")]
        public IActionResult saveInsight(cc_SnnInsight insight)
        {
            if (!readContext()) { return Unauthorized(); }
            var myInsight = new cc_SnnInsight();
            var mergeFields = new List<string> { "title", "ref_InsightType", "title", "author", "body", "keywords", "summary", "src","thumb" };
            if (string.IsNullOrEmpty(insight.id))
            {
                myInsight.id = Guid.NewGuid().ToString();
                myInsight.create_time = DateTime.Now;
                clib.mergeChanges(myInsight, insight, mergeFields);
                lib.companyHubDB.cc_SnnInsight.Add(myInsight);
            }
            else
            {
                myInsight = lib.companyHubDB.cc_SnnInsight.Where(i => i.id == insight.id).FirstOrDefault();
                if (myInsight == null) { return NotFound(); }
                clib.mergeChanges(myInsight, insight, mergeFields);
                insight.update_time = DateTime.Now;
                lib.companyHubDB.cc_SnnInsight.Update(myInsight);
            }
            lib.companyHubDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = insight;
            fillDataBags(insight);
            TempData["msgBox"] = myResponse.html;
            return RedirectToAction("details", new { id = insight.id });
        }

        [HttpDelete("/admin/Insight")]
        public apiResponse delete([FromQuery] string ids)
        {
            if (!readContext()) { return standardMessages.invalid; }
            var IDS = ids.Split(',').Select(a => a).ToList();
            var tobeRemoved = lib.companyHubDB.cc_SnnInsight.Where(a => IDS.Contains(a.id)).ToList();
            if (tobeRemoved.Any())
            {
                lib.companyHubDB.cc_SnnInsight.RemoveRange(tobeRemoved);
                lib.companyHubDB.SaveChanges();
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
