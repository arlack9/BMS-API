using Microsoft.AspNetCore.Mvc;

namespace BMS_API.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
