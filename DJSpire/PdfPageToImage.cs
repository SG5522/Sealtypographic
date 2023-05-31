using Spire.Pdf;
using System;
using System.IO;

namespace DJSpire
{
    public class PdfPageToImage
    {
        private string path;
        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        public string Path {
            get { return path; }
            set 
            { 
                path = value;
                if(!string.IsNullOrWhiteSpace(path))
                {
                    Document = new PdfDocument(path);
                    GetIndex();
                }
            } 
        }

        /// <summary>
        /// PDF頁次
        /// </summary>
        public int PageIndex { get; set; }

        public PdfDocument Document { get; set; }
        public PdfDocument IndexDocument { get; set; }


        public PdfPageToImage()
        {

        }

        private void GetIndex()
        {
            if (Document != null && PageIndex >= 0)
            {
                IndexDocument.InsertPage(Document, PageIndex);
            }
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
