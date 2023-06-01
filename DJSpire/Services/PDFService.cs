using DJSpire.Consts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using Spire.Pdf;
using System;
using System.IO;

namespace DJSpire.Services
{
    public class PDFService
    {
        private string path;
        private int pageIndex;

        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                if (!string.IsNullOrWhiteSpace(path))
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

        /// <summary>
        /// 圖片轉為Stream
        /// </summary>
        /// <returns></returns>
        public Stream GetPageImageStream(ImageType imageType = ImageType.Jpg)
        {
            Stream stream = new MemoryStream();
            string imageTypeString;
            switch (imageType)
            {
                case ImageType.Jpg:
                    imageTypeString = "jpg";
                    break;
                case ImageType.Png:
                    imageTypeString = "png";
                    break;
                case ImageType.Bmp:
                    imageTypeString = "bmp";
                    break;
                default:
                    imageTypeString = "jpg";
                    break;
            }
            IndexDocument.SaveToImageStream(0, stream, imageTypeString);
            return stream;
        }

        public string GetPageImageBase64(ImageType imageType = ImageType.Jpg)
        {
            Image image = Image.Load(GetPageImageStream(imageType), out IImageFormat format);
            return image.ToBase64String(format);            
        }

        /// <summary>
        /// PDF檔案轉Stream
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetPDFStream()
        {
            MemoryStream stream = new MemoryStream();
            Document.SaveToStream(stream);
            return stream;
        }

        /// <summary>
        /// PDF檔案轉Base64
        /// </summary>
        /// <returns></returns>
        public string GetPDFBase64()
        {
            //Stream to Array
            byte[] pdfBytes = GetPDFStream().ToArray();
            return $"{"data:application/pdf;base64,"}{Convert.ToBase64String(pdfBytes)}";
        }

        public int GetTotalPage()
        {
            return Document.Pages.Count;
        }
    }
}
