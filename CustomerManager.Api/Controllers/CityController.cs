using Microsoft.AspNetCore.Mvc;

namespace CustomerManager.Api.Controllers
{
    public class CityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
