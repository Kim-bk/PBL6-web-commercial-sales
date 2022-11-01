using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
