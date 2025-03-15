using Microsoft.AspNetCore.Mvc;

namespace CustomerManager.Api.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
