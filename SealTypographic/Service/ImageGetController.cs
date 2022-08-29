using DJLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SealTypographic.Models;

namespace SealTypographic.Service
{
    [Route("api/ImageGet")]
    [ApiController]
    public class ImageGetController : ControllerBase
    {
        private readonly string ScanImagePath = @".\wwwroot\Sealcard\";
        private readonly string SealTempPath = Path.GetTempPath() + @"\Seal\";

        /// <summary>
        /// 取得圖檔並顯示指定的圖
        /// </summary>
        /// <param name="imageName">圖片檔名</param>
        /// <returns></returns>                     
        /// GET api/imageWorks/imageName
        [HttpGet("{imageWorks}/{imageName}")]
        public string GetScanImage(int imageWorks,string imageName)
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
            ImageGetData imageGetData = new();
            ImageData? imageData = imageGetData.GetImageData(imagepath, imageName);
            if (imageData != null)
            {             
                //轉成image Base64
                return "data:" + imageData.ContentType + ";base64," + Convert.ToBase64String(imageData.Data, 0, imageData.Data.Length); ;
            }
            return "~/img/NoImage.svg";
        }
    }
}
