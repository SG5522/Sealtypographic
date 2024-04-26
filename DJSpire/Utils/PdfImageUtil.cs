using PDFtoImage;
using DJSpire.Consts;
using DJImageLib.Models;
using DJSpire.Models;
using Spire.Pdf;

namespace DJSpire.Utils
{
    /// <summary>
    /// PDF輸出檔名
    /// </summary>
    public static class PdfImageUtil
    {
        /// <summary>
        /// 取得PDF圖片與資訊
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="pageNo"></param>
        /// <param name="dpi"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static PdfPageImageInfo GetPdfPageImageInfo(string srcPath , int pageNo, int dpi = 300, PdfImageType imageType = PdfImageType.Jpg)
        {
            return GetPdfPageImageInfo(File.ReadAllBytes(srcPath), pageNo, dpi, imageType);
        }

        /// <summary>
        /// 取得PDF圖片與資訊
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="pageNo"></param>
        /// <param name="dpi"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static PdfPageImageInfo GetPdfPageImageInfo(Stream stream, int pageNo, int dpi = 300, PdfImageType imageType = PdfImageType.Jpg)
        {
            return GetPdfPageImageInfo(((MemoryStream)stream).ToArray(), pageNo, dpi, imageType);
        }

        /// <summary>
        /// 取得PDF與資訊
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pageNo"></param>
        /// <param name="dpi"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static PdfPageImageInfo GetPdfPageImageInfo(byte[] bytes, int pageNo, int dpi = 300, PdfImageType imageType = PdfImageType.Jpg)
        {
            PdfDocument pdfDocument = new(bytes);            
            ImageModel imageModel = new() { Base64 = GetPageImageBase64(bytes, pageNo, dpi, imageType) };

            return new PdfPageImageInfo()
            {
                Width = imageModel.ImageInfo!.Width,
                Height = imageModel.ImageInfo!.Height,
                PageNo = pageNo,
                TotalPage = pdfDocument.Pages.Count,
                ImageDataUrl = imageModel.DataUrl!               
            };
        }

        /// <summary>
        /// 將單頁PDF轉圖片
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public static string GetPageImageBase64(string srcPath, int pageNo, int dpi = 300, PdfImageType imageType = PdfImageType.Jpg)
        {
            return GetPageImageBase64(File.ReadAllBytes(srcPath), pageNo, dpi, imageType);
        }

        /// <summary>
        /// 將單頁PDF轉圖片
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="pageNo"></param>
        /// <param name="dpi"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static string GetPageImageBase64(Stream stream, int pageNo, int dpi = 300, PdfImageType imageType = PdfImageType.Jpg)
        {
            return GetPageImageBase64(((MemoryStream)stream).ToArray(), pageNo, dpi, imageType);
        }

        /// <summary>
        /// 將單頁PDF轉圖片
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pageNo"></param>
        /// <param name="dpi"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static string GetPageImageBase64(byte[] bytes, int pageNo, int dpi = 300 , PdfImageType imageType = PdfImageType.Jpg)
        {            
            MemoryStream memoryStream = new();
#pragma warning disable CA1416 // 驗證平台相容性            
            switch (imageType)
            {
                case PdfImageType.Jpg:                    
                    Conversion.SaveJpeg(memoryStream, bytes, null, pageNo - 1, options: new(dpi));
                    break;
                case PdfImageType.Png:
                    Conversion.SavePng(memoryStream, bytes, null, pageNo - 1, options: new(dpi));
                    break;
                case PdfImageType.Webp:
                    Conversion.SaveWebp(memoryStream, bytes, null, pageNo - 1, options: new(dpi));
                    break;
            }
#pragma warning restore CA1416 // 驗證平台相容性
            memoryStream.Position = 0;
            return Convert.ToBase64String(memoryStream.ToArray());
        }     
    }
}
