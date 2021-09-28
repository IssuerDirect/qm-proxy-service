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
    [Route("/admin/video"), Authorize, AutoValidateAntiforgeryToken]
    public class videoController : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        int pageSize = 50;


        public videoController(IConfiguration configuration, companyHubDB snnDB, platformDB _platformDB)
        {
            lib.companyHubDB = snnDB;
            lib.platformDB = _platformDB;
            clib.myConfiguration = configuration;
        }

        [HttpGet("/admin/video")] 
        public IActionResult Index(string keywords = null, int pageIndex = 0, bool json = false)
        {
            if (!readContext()) { return Unauthorized(); }
            myResponse = standardMessages.found;
            var videos = lib.companyHubDB.cc_SnnVideos.Where(a =>  keywords == null || a.title.Contains(keywords)).OrderByDescending(a=>a.create_time).ToList();
            myResponse.count = videos.Count();
            myResponse.data = videos.Skip(pageSize * pageIndex).Take(pageSize);
            myResponse.pageSize = pageSize;
            myResponse.pageIndex = pageIndex;
            if (json)
            {
                return Json(myResponse);
            }
       
            ViewData["videos"] = System.Text.Json.JsonSerializer.Serialize(myResponse);
            ViewData["title"] = "Homepage Videos";
            return View();
        }

        [HttpGet("/admin/video/details/{id?}")]
        public IActionResult details(int? id = null)
        {
            if (!readContext()) { return Unauthorized(); }
            cc_SnnVideos model = new cc_SnnVideos();
            if (id != null)
            {
                model = lib.companyHubDB.cc_SnnVideos.Where(i => i.id == id.Value).FirstOrDefault();
                if (model == null)
                {
                    return NotFound();
                }
                ViewData["title"] = "Edit Video: " + model.title;
            }
            else {
                ViewData["title"] = "Create Video";
            }

            return View("details", model);
        }

        [HttpPost("/admin/video/details/{id?}")]
        public IActionResult saveVideo([FromBody] cc_SnnVideos inputVideo)
        {
            if (!readContext()) { return Unauthorized(); }
            cc_SnnVideos dbVideo = new cc_SnnVideos();
            if (inputVideo.id == 0)
            {
                lib.companyHubDB.cc_SnnVideos.Add(inputVideo);
                lib.companyHubDB.SaveChanges();
                dbVideo = inputVideo;
            }
            else
            {
                dbVideo = lib.companyHubDB.cc_SnnVideos.Where(i => i.id == inputVideo.id).FirstOrDefault();
                if (dbVideo == null) { return  NotFound(); }
                dbVideo.title = inputVideo.title;
                dbVideo.author = inputVideo.author;
                dbVideo.link = inputVideo.link;
                lib.companyHubDB.cc_SnnVideos.Update(dbVideo);
            }
            
            myResponse = standardMessages.saved;
            myResponse.data = dbVideo;
            TempData["msgBox"] = myResponse.html;
            return RedirectToAction("details", dbVideo);
        }

        [HttpDelete("/admin/video")]
        public apiResponse delete([FromQuery] string ids)
        {
            if (!readContext()) { return standardMessages.invalid; }
            var IDS = ids.Split(',').Select(a => Convert.ToInt32(a)).ToList<int>();
            var tobeRemoved = lib.companyHubDB.cc_SnnVideos.Where(a => IDS.Contains(a.id)).ToList();
            if (tobeRemoved.Any())
            {
                lib.companyHubDB.cc_SnnVideos.RemoveRange(tobeRemoved);
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
