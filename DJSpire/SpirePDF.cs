using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.IO;

namespace DJSpire
{
    public class SpirePDF
    {
        public MemoryStream Load(string pdfFullName)
        {
            //Load pdf document
            PdfDocument pdfDocument = new PdfDocument();
            MemoryStream memoryStream = new MemoryStream();
            //Load pdf document
            pdfDocument.LoadFromFile(pdfFullName);
            //在STAND2.0的環境下使用SaveToStream 會需要使用SkiaSharp
            pdfDocument.SaveToStream(memoryStream);

            return memoryStream;
        }
        /// <summary>
        /// PDF轉PNG
        /// </summary>
        /// <param name="pdfFullName">完整檔案路徑名稱</param>
        /// <param name="pageNumber">頁次</param>
        /// <param name="pDFData">PDF資料(暫時無功能)</param>
        /// <returns></returns>
        public Stream LoadPDFToPNG(string pdfFullName, int pageNumber , PDFData pdfData)
        {
            //Load pdf document
            PdfDocument pdfDocument = new PdfDocument();
            pdfDocument.LoadFromFile(pdfFullName);
            //提供資料回傳
            pdfData.TotalPages = pdfDocument.Pages.Count;
            //save Pdf page to image 
            //在STAND2.0的環境下用到Stream會需要使用SkiaSharp
            return pdfDocument.SaveAsImage(pageNumber, PdfImageType.Bitmap);
        }
    }
}
