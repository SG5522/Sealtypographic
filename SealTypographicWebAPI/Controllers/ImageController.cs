using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models;
using Microsoft.Extensions.Options;
using DJLib;
using DJLib.Models;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得圖片(Base64)
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly SealConfigPath sealConfig;

        private readonly ImageSharpService imageSharpService;



        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="options"></param>
        /// <param name="imageSharpService"></param>
        public ImageController(IOptionsMonitor<SealConfigPath> options, ImageSharpService imageSharpService)
        {
            sealConfig = options.CurrentValue;            
            this.imageSharpService = imageSharpService;
        }
       
        /// <summary>
        /// 取得Base64字串
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        [HttpGet("{imagePath}")]
        public string GetImageBase64(string imagePath)
        {
            //測試存圖用
            //string base64 = ImageSharpUtil.PathImageFileToBase64(imagePath);
            //ImageSharpUtil.Base64ToSaveImage(base64);

            return ImageSharpUtil.PathImageFileToBase64(imagePath);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagePathtest"></param>
        [HttpGet("imagePathtest")]
        public void Test(string imagePathtest)
        {
            
            imagePathtest = @"C:\\DJimage\\TmpScanImage\\3010017_2208191313553_F.jpg";
            SealConfigPath sealConfigPath = sealConfig;            

            string base64 = ImageSharpUtil.PathImageFileToBase64(imagePathtest);
            imageSharpService.SaveBase64ToFile(SealType.Customer, base64, "AAA000");
            //SaveImageInfo saveImageInfo = new()
            //{
            //    Filename = "test.jpg",
            //    Folder = sealConfigPath.SealImagePath + sealConfigPath.Customer
            //};

            //ImageSharpUtil.Base64ToSaveImage(base64, saveImageInfo);
        }
    }
}
