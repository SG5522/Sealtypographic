using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        public PdfImageType ImageFormat { get; private set; }

        public PdfPageToImage()
        {

        }

        public string GetImageBase64(PdfPageToImage pdfPageToImage)
        {
            string imageBase64 = string.Empty;

            //Open pdf document
            PdfDocument pdf = new PdfDocument();
            pdf.LoadFromFile(pdfPageToImage.Path);


            return imageBase64;

        }
    }
}
