using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using net3000;
using net3000.common.models;
using System;
using System.Linq;

namespace snn.Controllers
{
    public class videoController : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        int pageSize = 50;


        public videoController(IConfiguration configuration, companyHubDB snnDB)
        {
            lib.companyHubDB = snnDB;
            lib.config = configuration;
            clib.myConfiguration=configuration;
        }
        [HttpGet("/admin/videos")] 
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

        [HttpGet("/admin/latestvideo")]
        public apiResponse latestvideo()
        {
            if (!readContext()) { return standardMessages.invalid; }
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

        [HttpPost("/admin/video")]
        public apiResponse saveVideo([FromBody] cc_SnnVideos video)
        {
            if (!readContext()) { return standardMessages.unauthorized; }
            lib.companyHubDB.cc_SnnVideos.Add(video);
            lib.companyHubDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = video;
            return myResponse;
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
            clib.myUser(User);
            if (clib.account <= 0) { return false; }
            return true;
        }

    }
}
