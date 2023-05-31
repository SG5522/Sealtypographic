using Spire.Pdf;
using System;
using System.IO;

namespace DJSpire
{
    public class PdfView
    {
        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// PDF頁次
        /// </summary>
        public int PageIndex { get; set; }

        private PdfDocument Document { get; set; }

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="path">預先輸入路徑</param>
        public PdfView(string path)
        {
            this.Path = path;
            LoadDocument();
        }        

        public void LoadDocument()
        {
            Document = new PdfDocument();
            Document.LoadFromFile(Path);
        }

        public int GetTotalPage()
        {
            return Document.Pages.Count;
        }

        /// <summary>
        /// PDF檔案轉Stream
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetPdfStream()
        {
            MemoryStream stream = new MemoryStream();
            Document.SaveToStream(stream);
            return stream;
        }


        
        /// <summary>
        /// PDF檔案轉Base64
        /// </summary>
        /// <returns></returns>
        public string GetBase64()
        {
            //Stream to Array
            byte[] pdfBytes = GetPdfStream().ToArray();
            return Convert.ToBase64String(pdfBytes);
        }

        /// <summary>
        /// PDF轉base64 To WebApi
        /// </summary>
        /// <returns></returns>
        public string GetBase64ToWebApi()
        {
            //toBase64
            return $"{"data:application/pdf;base64,"}{GetBase64()}";
        }
    }
}
