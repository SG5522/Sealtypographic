using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.IO;
using System;

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
            MemoryStream stream = (MemoryStream)pdf.SaveAsImage(pdfPageToImage.PageIndex);
            
            byte[] imagebytes = stream.ToArray();

            return Convert.ToBase64String(imagebytes); ;
        }
    }
}
