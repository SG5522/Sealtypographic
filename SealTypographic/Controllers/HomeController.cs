using Microsoft.AspNetCore.Mvc;
using SealTypographic.Models;
using System.Diagnostics;
using DJSpireNET6;
using System.Drawing.Imaging;

namespace SealTypographic.Controllers
{
    public class HomeController : Controller
    {
        //private SpirePDF spirePDF = new();
        private SpirePDF spirePDF = new();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*
        public IActionResult Privacy()
        {
            return View();
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //[HttpPost]
        public IActionResult PdfOpen()
        {
            //string FileName= "D:\\temp\\myPdf.pdf";
            string pdfPath = @"D:\works\SealTypographic\SealTypographic\wwwroot\pdf\test.pdf";

            //return File(stream, "application/pdf");
            PDFData pdfData = new();
            Stream stream = spirePDF.PdfLoadToPNG(pdfPath, 1, pdfData, ImageFormat.Png);            
            //int pagetotal = pdfData.PDFTotalPage;
            return File(stream, "image/png");
        }
    }
}