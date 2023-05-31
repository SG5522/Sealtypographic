using Spire.Pdf;
using System;
using System.IO;

namespace DJSpire
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
        

        public PdfPageToImage()
        {

        }

        public static string GetImageBase64(PdfPageToImage pdfPageToImage)
        {
            //Open pdf document
            PdfDocument pdf = new PdfDocument();
            pdf.LoadFromFile(pdfPageToImage.Path);
            Stream stream = new MemoryStream();
            pdf.SaveToImageStream(pdfPageToImage.PageIndex, stream, "png");
            //Stream stream = pdf.SaveAsImage(pdfPageToImage.PageIndex);
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            byte[] imagebytes = memoryStream.ToArray();

            return $"{"data:image/png;base64,"}{Convert.ToBase64String(imagebytes)}";
        }
    }
}
