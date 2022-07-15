using Microsoft.AspNetCore.Mvc;

namespace SealTypographicDemo.Controllers
{
    public class PasswordController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
