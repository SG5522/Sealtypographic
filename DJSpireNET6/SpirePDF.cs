using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using System.Drawing.Imaging;


namespace DJSpireNET6
{
    public class SpirePDF
    {
        private readonly PdfDocument pdfDocument = new PdfDocument();
        //public PdfDocument pdfDocument = new PdfDocument();

        public MemoryStream PdfLoad(string pdfFullName)
        {
            MemoryStream memoryStream = new MemoryStream();
            
            //Load pdf document
            pdfDocument.LoadFromFile(pdfFullName);
      
            
            pdfDocument.SaveToStream(memoryStream);

            return memoryStream;
        }

        /// <summary>
        /// PDF轉PNG
        /// </summary>
        /// <param name="pdfFullName">PDF檔案位置</param>
        /// <param name="page">要轉換的頁碼</param>
        /// <param name="pDFData">PDF的資料(暫時用不到)</param>
        /// <param name="imageFormat">轉換圖檔型態(只支援WINDOWS)</param>
        /// <returns></returns>
        public Stream PdfLoadToPNG(string pdfFullName, int page, PDFData pDFData, ImageFormat imageFormat)
        {
            //Load pdf document
            pdfDocument.LoadFromFile(pdfFullName);
            //提供資料回傳
            pDFData.PDFTotalPage = pdfDocument.Pages.Count;
            //save Pdf page to image
            Image image = pdfDocument.SaveAsImage(page,PdfImageType.Bitmap);            
            MemoryStream MemoryStream = new();
            //只能在windows環境下使用
            image.Save(MemoryStream, imageFormat);

            MemoryStream.Position = 0;
            return MemoryStream;            
        }
        public void PdfDocumentClose()
        {
            pdfDocument.Close();
        }
    }
}
