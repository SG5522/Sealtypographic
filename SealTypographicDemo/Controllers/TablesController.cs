using Microsoft.AspNetCore.Mvc;

namespace SealTypographicDemo.Controllers
{
    public class TablesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
