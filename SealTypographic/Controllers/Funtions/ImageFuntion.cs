using DJLib;
using SealTypographic.Models;

namespace SealTypographic.Controllers.Funtions
{
    public class ImageFuntion
    {
        private readonly float HeightScale = 0.25f;
        private readonly float WidthScale = 0.25f;
        /// <summary>
        /// 取得指定檔名圖檔資料
        /// </summary>
        /// <param name="imagePath">圖檔位置</param>
        /// <param name="imageName">圖片檔名</param>
        /// <returns></returns>
        public ImageData? GetData(string imagePath, string imageName)
        {
            ImageData imageData = new()
            {
                FileName = imageName,
                ContentType = GetType(imageName),
                Data = ImageResize.ReDrawImgToBytes(imagePath + imageName, WidthScale, HeightScale),//縮放圖檔並轉成Bytes
            };
            if (imageData.Data != null)
            {
                return imageData;
            }
            else
            {
                return null;
            }
        }

        public static string GetType(string fileName)
        {
            string imageType = "";
            string fileExtension = fileName[fileName.LastIndexOf(".")..];
            switch (fileExtension)
            {
                case ".apng":
                    imageType = "image/apng";
                    break;
                case ".avif":
                    imageType = "image/avif";
                    break;
                case ".bmp":
                    imageType = "image/bmp";
                    break;
                case ".gif":
                    imageType = "image/gif";
                    break;
                case ".jpg":
                    imageType = "image/jpeg";
                    break;
                case ".jpeg":
                    imageType = "image/jpeg";
                    break;
                case ".jfif":
                    imageType = "image/jpeg";
                    break;
                case ".pjpeg":
                    imageType = "image/jpeg";
                    break;
                case ".pjp":
                    imageType = "image/jpeg";
                    break;
                case ".png":
                    imageType = "image/png";
                    break;
                case ".svg":
                    imageType = "image/svg+xml";
                    break;
                case ".tif":
                    imageType = "image/tiff";
                    break;
                case ".tiff":
                    imageType = "image/tiff";
                    break;
                case ".webp":
                    imageType = "image/webp";
                    break;
            }
            return imageType;
        }
    }
}
