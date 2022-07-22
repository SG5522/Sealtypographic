using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.IO;

namespace DJSpire
{
    public class SpirePDF
    {
        private readonly PdfDocument pdfDocument = new PdfDocument();
        //public PdfDocument pdfDocument = new PdfDocument();
        public void PDFOpen(string pdfPath)
        {
            //Load pdf document
            
            pdfDocument.LoadFromFile(pdfPath);

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
        public MemoryStream PdfLoad(string pdfPath)
        {
            MemoryStream memoryStream = new MemoryStream();
            
            //Load pdf document
            pdfDocument.LoadFromFile(pdfPath);
      

            //在STAND2.0的環境下使用SaveToStream 會需要使用SkiaSharp
            pdfDocument.SaveToStream(memoryStream);

            return memoryStream;
        }
        public Stream PdfLoadToPNG(string pdfPath, int page , PDFData pDFData)
        {
            //Load pdf document
            pdfDocument.LoadFromFile(pdfPath);
            //提供資料回傳
            pDFData.PDFTotalPage = pdfDocument.Pages.Count;
            //save Pdf page to image           
            return pdfDocument.SaveAsImage(page, PdfImageType.Bitmap);
        }
        public void PdfDocumentClose()
        {
            pdfDocument.Close();
        }
    }
}
