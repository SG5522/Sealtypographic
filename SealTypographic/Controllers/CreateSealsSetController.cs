using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace SealTypographic.Controllers
{
    public class CreateSealsSetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SealImageSelect()
        {
            return View();
        }
        public IActionResult GetImage(string imageName)
        {
            string imagePath = @"D:\Sealcard\";            
            byte[] image = GetBytesFromImage(imagePath + imageName);
            if(image != null)
            {
                return File(image, "image/jpg");                
            }
            else
            {
                return View("Error");
            }
        }
        public byte[] GetBytesFromImage(string imagePath)
        {
            try
            {
                FileStream fs = new(imagePath, FileMode.Open, FileAccess.Read);

                int length = (int)fs.Length;

                byte[] image = new byte[length];

                fs.Read(image, 0, length);

                fs.Close();

                return image;
            }
            catch (Exception)
            {
                return null;
            }          
        }
    }
}
