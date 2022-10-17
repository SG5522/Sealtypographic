using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Service;
using SealTypographicWebAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得圖片(Base64)
    /// </summary>
    [Route("ImageGet")]
    [Produces("application/json")]
    [ApiController]
    public class ImageGetController : ControllerBase
    {
        private readonly string ScanImagePath = @"C:\DJimage\TestSealcard\";
        private readonly string SealTempPath = Path.GetTempPath() + @"\Seal\";

        /// <summary>
        /// 取得圖檔並顯示指定的圖
        /// </summary>
        /// <param name="imageWorks">選擇使用路徑 1.掃描圖檔位置 2.印鑑暫存位置(本機暫存檔位置)</param>
        /// <param name="imageName">圖片檔名</param>
        /// <returns>取得圖片(Base64)</returns>                     
        /// GET api/imageWorks/imageName
        [HttpGet("{imageWorks}/{imageName}")]
        public string GetScanImage(int imageWorks, string imageName)
        {
            string imagepath;
            switch (imageWorks)
            {
                case 1:
                    imagepath = ScanImagePath;
                    break;
                case 2:
                    imagepath = SealTempPath;
                    break;
                default:
                    imagepath = ScanImagePath;
                    break;
            }
            ImageFuntion imageGetData = new();
            ImageData? imageData = imageGetData.GetData(imagepath, imageName);
            if (imageData != null)
            {
                //轉成image Base64
                return "data:" + imageData.ContentType + ";base64," + Convert.ToBase64String(imageData.Data, 0, imageData.Data.Length);
            }
            return "~/img/NoImage.svg";
        }
    }
}
