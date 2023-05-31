using Spire.Pdf;
using System;
using System.IO;

namespace DJSpire
{
    public class PdfPageToImage
    {
        private string path;
        private int pageIndex;

        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        public string Path 
        { 
            get{ return path; }
            set
            {
                path = value;
                if(!string.IsNullOrWhiteSpace(path))
                {
                    Document = new PdfDocument(path);
                }                
            } 
        }

        /// <summary>
        /// PDF頁次
        /// </summary>
        public int PageIndex 
        {
            get { return pageIndex; }
            set
            { 
                pageIndex = value;
                if (Document != null)
                {
                    IndexDocument = new PdfDocument();
                    IndexDocument.InsertPage(Document, PageIndex);
                }
            }
        }
        
        /// <summary>
        /// 原PDF檔
        /// </summary>
        public PdfDocument Document { get; set; }

        /// <summary>
        /// 指定頁次的PDF檔
        /// </summary>
        public PdfDocument IndexDocument { get; set; }


        public Stream GetImageStream()
        {
            Stream stream = new MemoryStream();            
            IndexDocument.SaveToImageStream(0, stream, "png");
            return stream;
        }
    }
}
