using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using net3000;
using net3000.common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snn.Controllers
{
    public class snn_eventsController : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        int pageSize = 24;

        public snn_eventsController(IConfiguration _config, companyHubDB _companyHubDB, peopleHubDB _peopleHubDB,platformDB _platformDB)
        {
            lib.config = _config;
            lib.companyHubDB = _companyHubDB;
            lib.peopleHubDB = _peopleHubDB;
            lib.platformDB = _platformDB;
            clib.myConfiguration = _config;
        }

        [HttpGet("/admin/events")]
        public IActionResult Index(string keywords = null, int pageIndex = 0, bool json = false)
        {
            if (!readContext()) { return Unauthorized(); }
            myResponse = standardMessages.found;
            IQueryable<cc_Conference> eventQuery = lib.companyHubDB.cc_Conference; 
            if (!string.IsNullOrEmpty(keywords))
            {
                eventQuery = eventQuery.Where(a => a.title.ToLower().Contains(keywords.ToLower()));
            }
            myResponse.count = eventQuery.Count();
            myResponse.data = eventQuery.OrderByDescending(a => a.id).ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList(); ;
            myResponse.pageSize = pageSize;
            myResponse.pageIndex = pageIndex;
            if (json)
            {
                return Json(myResponse);
            }
            ViewData["events"] = System.Text.Json.JsonSerializer.Serialize(myResponse);
            return View();
        }


        [HttpGet("/admin/events/details/{id?}")]
        public IActionResult details(int? id = null)
        {
            if (!readContext()) { return Unauthorized(); }
            cc_Conference model = new cc_Conference();
            if (id.HasValue)
            {
                model = lib.companyHubDB.cc_Conference.Where(i => i.id == id).Include(c => c.company).FirstOrDefault();
                if (model == null)
                {
                    return NotFound();
                }
                ViewData["title"] = $"Updateing Event {model.title}";
            }
            else
            {
                ViewData["title"] = "Create Event";
            }
            setBags();
            return View("details", model);
        }
        void setBags()
        {
            ViewBag.status = lib.platformDB.ref_Status.Select(a => new SelectListItem() { Value = a.id.ToString(), Text = a.name }).ToList();
            ViewBag.companies = lib.companyHubDB.ci_Company.Select(a => new SelectListItem() { Value = a.id.ToString(), Text = a.name }).ToList();

            ViewBag.industries = lib.companyHubDB.cc_Conference.Where(c => !string.IsNullOrEmpty(c.industry)).GroupBy(c => c.industry).OrderBy(o => o.Key).Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Key }).ToList();
        }
        [HttpPost("/admin/events/details")]
        public IActionResult saveevent(cc_Conference _event)
        {
            if (!readContext()) { return Unauthorized(); }
            var myevent = new cc_Conference();
            if (_event.id == 0)
            {
                lib.companyHubDB.cc_Conference.Add(_event);
            }
            else
            {
                myevent = lib.companyHubDB.cc_Conference.Where(i => i.id == _event.id).FirstOrDefault();
                if (myevent == null) { return NotFound(); }
                clib.mergeChanges(myevent, _event);
                lib.companyHubDB.cc_Conference.Update(myevent);
            }
            lib.companyHubDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = _event; 
            TempData["msgBox"] = myResponse.html;
            return RedirectToAction("details", new { id = _event.id });
        }

        [HttpDelete("/admin/event")]
        public apiResponse delete([FromQuery] string ids)
        {
            if (!readContext()) { return standardMessages.invalid; }
            var IDS = ids.Split(',').Select(a => Convert.ToInt32(a)).ToList<int>();
            var tobeRemoved = lib.companyHubDB.cc_Conference.Where(a => IDS.Contains(a.id)).ToList();
            if (tobeRemoved.Any())
            {
                lib.companyHubDB.cc_Conference.RemoveRange(tobeRemoved);
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
