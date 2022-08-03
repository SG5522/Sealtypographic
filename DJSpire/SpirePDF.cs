using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.IO;

namespace DJSpire
{
    public class SpirePDF
    {
        private readonly PdfDocument pdfDocument = new PdfDocument();
        //public PdfDocument pdfDocument = new PdfDocument();

        public void PDFOpen(string pdfFullName)
        {
            //Load pdf document
            
            pdfDocument.LoadFromFile(pdfFullName);

            //Set view reference
            pdfDocument.ViewerPreferences.CenterWindow = true;
            pdfDocument.ViewerPreferences.DisplayTitle = false;
            pdfDocument.ViewerPreferences.FitWindow = false;
            pdfDocument.ViewerPreferences.HideMenubar = true;
            pdfDocument.ViewerPreferences.HideToolbar = true;
            pdfDocument.ViewerPreferences.PageLayout = PdfPageLayout.SinglePage;

            //Save pdf file
            //pdfDocument.SaveToFile("ViewerPreference_result.pdf");
            //pdfDocument.Close();

            //Launch the Pdf file
            //PDFDocumentViewer("ViewerPreference_result.pdf");
            
        }
        public MemoryStream PdfLoad(string pdfFullName)
        {
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
        /// <param name="page">頁次</param>
        /// <param name="pDFData">PDF資料(暫時無功能)</param>
        /// <returns></returns>
        public Stream PdfLoadToPNG(string pdfFullName, int page , PDFData pDFData)
        {
            //Load pdf document
            pdfDocument.LoadFromFile(pdfFullName);
            //提供資料回傳
            pDFData.PDFTotalPage = pdfDocument.Pages.Count;

            //save Pdf page to image           
            //在STAND2.0的環境下用到Stream會需要使用SkiaSharp
            return pdfDocument.SaveAsImage(page, PdfImageType.Bitmap);
        }
        public void PdfDocumentClose()
        {
            pdfDocument.Close();
        }
    }
}
