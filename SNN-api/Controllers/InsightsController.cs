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
        int pageSize = 24;

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
                insightQuery = insightQuery.Where(a => a.src.ToLower().Contains(keywords.ToLower()));
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

        void fillDataBags(string insightID = null)
        {
            if (insightID != null)
            {
                if (insightID == "0")
                {
                    ViewData["title"] = "Create Insight";
                }
                else
                {
                    ViewData["title"] = "Edit Insight# " + insightID;
                }
            }
           // ViewBag.statuses = lib..ref_Status.Select(a => new SelectListItem() { Value = a.id.ToString(), Text = a.name }).ToList();
            ViewBag.types = lib.companyHubDB.ref_InsightType.Select(a => new SelectListItem() { Value = a.id.ToString(), Text = a.name }).ToList();
        }

        [HttpGet("/admin/Insights/details/{id?}")]
        public IActionResult details(string id = null)
        {
            if (!readContext()) { return Unauthorized(); }
            cc_SnnInsight model = new cc_SnnInsight();
            if (id!=null)
            {
                model = lib.companyHubDB.cc_SnnInsight.Where(i => i.id == id).FirstOrDefault();
                if (model == null)
                {
                    return NotFound();
                }
            }
            fillDataBags(model.id);
            return View("details", model);
        }

        [HttpPost("/admin/Insights/details")]
        public IActionResult saveInsight(cc_SnnInsight insight)
        {
            if (!readContext()) { return Unauthorized(); }
            var myInsight = new cc_SnnInsight();
            if (insight.id == null)
            {
                insight.create_time = DateTime.Now;
                lib.companyHubDB.cc_SnnInsight.Add(insight);
            }
            else
            {
                myInsight = lib.companyHubDB.cc_SnnInsight.Where(i => i.id == insight.id).FirstOrDefault();
                if (myInsight == null) { return NotFound(); }
                clib.mergeChanges(myInsight, insight, new List<string> { "title", "ref_InsightType", "headline","author","body" , "keywords", "summary" });
                insight.update_time = DateTime.Now;
                lib.companyHubDB.cc_SnnInsight.Update(myInsight);
            }
            lib.companyHubDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = insight;
            fillDataBags(insight.id);
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

        #region content
        [HttpGet("/admin/Insights/content")]
        public IActionResult content(string keywords = null, int? type = null, int? status = null, int pageIndex = 0, bool json = false)
        {
            if (!readContext()) { return Unauthorized(); }
            myResponse = standardMessages.found;
            IQueryable<snn_InsightContent> insightQuery = lib.platformDB.snn_InsightContent.Where(i => i.id > 86).Include(Z => Z.ref_InsightType).Include(a => a.ref_StatusObject);
            if (type.HasValue)
            {
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
        [HttpGet("/admin/Insights/contentDetails/{id?}")]
        public IActionResult contentDetails(int? id = null)
        {
            if (!readContext()) { return Unauthorized(); }
            snn_InsightContent model = new snn_InsightContent();
            if (id.HasValue)
            {
                model = lib.platformDB.snn_InsightContent.Where(i => i.id == id).FirstOrDefault();
                if (model == null)
                {
                    return NotFound();
                }
            }
            fillDataBags(model.id.ToString());
            return View("contentDetails", model);
        }

        [HttpPost("/admin/Insights/contentDetails")]
        public IActionResult saveContentDetails(snn_InsightContent insight)
        {
            if (!readContext()) { return Unauthorized(); }
            var myInsight = new snn_InsightContent();
            if (insight.id == 0)
            {
                lib.platformDB.snn_InsightContent.Add(insight);
            }
            else
            {
                myInsight = lib.platformDB.snn_InsightContent.Where(i => i.id == insight.id).FirstOrDefault();
                if (myInsight == null) { return NotFound(); }
                clib.mergeChanges(myInsight, insight);
                lib.platformDB.snn_InsightContent.Update(myInsight);
            }
            lib.platformDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = insight;
            fillDataBags(insight.id.ToString());
            TempData["msgBox"] = myResponse.html;
            return RedirectToAction("contentDetails", new { id = insight.id });
        }
        [HttpDelete("/admin/InsightContent")]
        public apiResponse deleteContent([FromQuery] string ids)
        {
            if (!readContext()) { return standardMessages.invalid; }
            var IDS = ids.Split(',').Select(a => Convert.ToInt32(a)).ToList<int>();
            var tobeRemoved = lib.platformDB.snn_InsightContent.Where(a => IDS.Contains(a.id)).ToList();
            if (tobeRemoved.Any())
            {
                lib.platformDB.snn_InsightContent.RemoveRange(tobeRemoved);
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
        #endregion
        bool readContext()
        {
            lib.myUser(User);
            if (lib.user.id <= 0) { return false; }
            return true;
        }
    }
}
