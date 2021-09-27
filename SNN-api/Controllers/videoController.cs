using Microsoft.AspNetCore.Mvc;
using net3000.common.models;
using net3000;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace snn.Controllers
{
    public class videoController : Controller
    {
        apiResponse myResponse;
        net3000.common.lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        public videoController(IConfiguration _config, companyHubDB _companyHubDB, peopleHubDB _peopleHubDB, platformDB _platformDB)
        {
            lib.config = _config;
            lib.companyHubDB = _companyHubDB;
            lib.peopleHubDB = _peopleHubDB;
            lib.platformDB = _platformDB;
            clib.myConfiguration = _config;
        }
        [HttpGet("/admin/videos")]
        public IActionResult Index()
        {
            ViewData["title"] = "Homepage Videos";
            return View();
        }
        [HttpGet("/admin/latestvideo")]
        public apiResponse latestvideo()
        {
            //if (!readContext()) { return standardMessages.invalid; }
            var vedio = lib.companyHubDB.cc_SnnVideos.OrderByDescending(v => v.id).FirstOrDefault();
            if (vedio != null)
            {
                myResponse = standardMessages.found;
                myResponse.data = vedio;
                return myResponse;
            }
            myResponse = standardMessages.notFound;
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
