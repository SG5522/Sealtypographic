using DBEntities.Consts;
using DBEntities.Entities;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using Spire.Pdf;

namespace SealTypographicWebAPI.Utils.Pdf
{
    /// <summary>
    /// PDF輸出檔名
    /// </summary>
    public static class PdfImageUtil
    {
        /// <summary>
        /// 取得輸出PDF預設之檔名 (目前依勤業為主)
        /// </summary>
        /// <param name="code">客戶編號</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        public static string GetName(string code, QuarterYear quarter)
        {
            return $"{code}{"A4"}{QuarterUtil.GetTaiwanYearQuarter(quarter)}";
        }

        /// <summary>
        /// 取得PDF圖片與資訊
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="pdfPath"></param>
        /// <param name="pageNumber"></param>        
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static PDFImageInfo GetPageImageInfo(string pdfPath, int pageNumber, double scale = 1, ImageType imageType = ImageType.Png)
        {
            return GetPageImageInfo(File.ReadAllBytes(pdfPath), pageNumber, scale, imageType);
        }

        /// <summary>
        /// 取得PDF圖片與資訊
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="pdfBytes"></param>
        /// <param name="pageNumber"></param>        
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static PDFImageInfo GetPageImageInfo(byte[] pdfBytes, int pageNumber, double scale = 1, ImageType imageType = ImageType.Png)
        {
            PdfDocument pdfDocument = new(pdfBytes);
            byte[] pdfImageBytes = GetPageImageBytes(pdfDocument, pageNumber, imageType);
            Image image = Image.Load(pdfImageBytes);
            IImageFormat imageFormat = Image.DetectFormat(pdfImageBytes);

            if (scale != 1.0)
            {
                int width = (int)(image.Width * scale);
                int height = (int)(image.Height * scale);
                image.Mutate(delegate (IImageProcessingContext x)
                {
                    x.Resize(width, height);
                });
            }

            return new PDFImageInfo()
            {
                Width = image.Width,
                Height = image.Height,
                TotalPage = pdfDocument.Pages.Count,
                ImageBase64 = image.ToBase64String(imageFormat)
            };
        }

        /// <summary>
        /// 單頁PDF轉為ImageBytes
        /// </summary>
        /// <param name="pdfDocument">PdfSpire的Class</param>
        /// <param name="pageNumber"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static byte[] GetPageImageBytes(PdfDocument pdfDocument, int pageNumber, ImageType imageType = ImageType.Png)
        {
            return ((MemoryStream)GetPageImageStream(pdfDocument, pageNumber, imageType)).ToArray();
        }

        /// <summary>
        /// 單頁PDF轉為ImageStream
        /// </summary>
        /// <param name="pdfDocument">PdfSpire的Class</param>
        /// <param name="pageNumber"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static Stream GetPageImageStream(PdfDocument pdfDocument, int pageNumber, ImageType imageType = ImageType.Png)
        {
            Stream result = new MemoryStream();
            string imageTypeString = imageType switch
            {
                ImageType.Jpg => "jpg",
                ImageType.Png => "png",
                ImageType.Bmp => "bmp",
                _ => "jpg",
            };

            //pdf頁次從0開始算 外部頁次號碼都是當下頁次所以要先-1
            pageNumber--;

            pdfDocument.SaveToImageStream(pageNumber >= 0 ? pageNumber : 0, result, imageTypeString);
            return result;
        }

        /// <summary>
        /// 依照圖像類別決定縮放大小
        /// </summary>
        /// <param name="subSealType"></param>
        /// <returns></returns>
        public static float GetImageScale(SubSealType subSealType)
        {
            float scale = 0;
            switch (subSealType)
            {
                case SubSealType.Company:
                    scale = PDFImageScaleConsts.CompanySeal;
                    break;
                case SubSealType.President:
                    scale = PDFImageScaleConsts.PresidentSeal;
                    break;
                case SubSealType.Manager:
                    scale = PDFImageScaleConsts.ManagerSeal;
                    break;
                case SubSealType.AccountingDirector:
                    scale = PDFImageScaleConsts.AccountingDirectorSeal;
                    break;
                case SubSealType.Seal:
                    scale = PDFImageScaleConsts.AccountingSeal;
                    break;
                case SubSealType.CHSign:
                    scale = PDFImageScaleConsts.CHSign;
                    break;
                case SubSealType.ENSign:
                    scale = PDFImageScaleConsts.ENSign;
                    break;
                case SubSealType.OldSign:
                    scale = PDFImageScaleConsts.OldSign;
                    break;
                case SubSealType.Letterhead:
                    scale = PDFImageScaleConsts.LetterheadImage;
                    break;
                case SubSealType.TemporarySeal:
                    scale = PDFImageScaleConsts.TemporarySeal;
                    break;
                case SubSealType.Other:
                    scale = PDFImageScaleConsts.Other;
                    break;
            }
            return scale;
        }
    }
}
