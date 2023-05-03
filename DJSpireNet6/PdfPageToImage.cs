using Spire.Pdf;
using System.Drawing;
using System.Drawing.Imaging;

namespace DJSpireNet6
{
    public class PdfPageToImage
    {
        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// PDF頁次
        /// </summary>
        public int PageIndex { get; set; }

        public static string GetImageBase64(PdfPageToImage pdfPageToImage)
        {
            //Open pdf document
            PdfDocument pdf = new();
            pdf.LoadFromFile(pdfPageToImage.Path);

            Image image = pdf.SaveAsImage(pdfPageToImage.PageIndex);
            MemoryStream memoryStream = new();
            image.Save(memoryStream, ImageFormat.Png);
            byte[] imagebytes = memoryStream.ToArray();
            
            return $"{"data:image/png;base64,"}{Convert.ToBase64String(imagebytes)}";
        }
    }
}