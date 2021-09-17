using Microsoft.AspNetCore.Mvc;
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
    [Route("/Insights")]
    public class Insights : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        private readonly platformDB platformDB;
        int pageSize = 50;
        public Insights(IConfiguration configuration, platformDB snnDB)
        {
            platformDB  = snnDB ; 
            clib.myConfiguration = configuration;
        }
        [HttpGet("/snn_Insights/Index")]
        public IActionResult Index(string keywords=null, int Type = 0, int status=-90, int pageIndex = 0)
        {
            if (!readContext()) { return Unauthorized(); }
            myResponse = standardMessages.found;
          var insights=  platformDB.snn_Insight.Where(a => (Type == 0 || a.type == Type) && (status == -90 || a.ref_Status == status) && (keywords == null || a.title.Contains(keywords))).
                Include(Z=>Z.ref_InsightType).Include(a=>a.ref_Statuses).Skip(pageSize * pageIndex).Take(pageSize).ToList();
                myResponse.count = insights.Count();
                    myResponse.data = insights;
                    myResponse.pageSize = pageSize;
                    myResponse.pageIndex = pageIndex;
            ViewData["insights"]= Json(myResponse);
            ViewBag.statuses = platformDB.ref_Status.Select(a => new { a.id, a.name }).ToDictionary(z => z.id, z => z.name);
            ViewBag.types = platformDB.ref_InsightType.Select(a => new { a.id, a.name }).ToDictionary(z => z.id, z => z.name);
            return View();
        }
        [HttpDelete("/snn_Insights/{ids}")]
        public apiResponse delete([FromQuery] string ids)
        {
            if (!readContext()) { return standardMessages.invalid; }
            var IDS = ids.Split(',').Select(a=>Convert.ToInt32( a)).ToList<int>() ;
         var tobeRemoved=   platformDB.snn_Insight.Where(a => IDS.Contains(a.id)).ToList();
            if(tobeRemoved.Any())
            {
                platformDB.snn_Insight.RemoveRange(tobeRemoved);
                platformDB.SaveChanges();
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
