using Microsoft.AspNetCore.Mvc; 

namespace snn.Controllers
{
    public class videoController : Controller
    {
        [HttpGet("/admin/videos")]
        public IActionResult Index()
        {
            ViewData["title"] = "Homepage Videos";
            return View();
        }
    }
}
