using DJSpire.Consts;
using DJSpire.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DJSpire.Services
{
    public class PDFService
    {
        private string pdfpath;
        private int pageIndex;

        /// <summary>
        /// 原PDF檔
        /// </summary>
        public PdfDocument Document { get; set; }

        /// <summary>
        /// 指定頁次的PDF檔
        /// </summary>
        public PdfDocument IndexDocument { get; set; }

        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        public string PDFPath
        {
            get { return pdfpath; }
            set
            {
                pdfpath = value;
                if (!string.IsNullOrWhiteSpace(pdfpath))
                {
                    Document = new PdfDocument(pdfpath);
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
                pageIndex = value - 1;
                if (Document != null & pageIndex >= 0)
                {
                    IndexDocument = new PdfDocument();
                    IndexDocument.InsertPage(Document, pageIndex);
                }
            }
        }


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
        /// PDF檔案轉Base64
        /// </summary>
        /// <returns></returns>
        public string GetPDFBase64()
        {
            MemoryStream stream = new MemoryStream();
            Document.SaveToStream(stream);            
            return GetMemoryStreamToBase64(stream);
        }

        public string GetPDFPageBase64()
        {
            MemoryStream stream = new MemoryStream();
            IndexDocument.SaveToStream(stream);
            return GetMemoryStreamToBase64(stream);
        }


        public int GetTotalPage()
        {
            return Document.Pages.Count;
        }

        public string GetEditPDFBase64(List<EditPage> editPages, bool isBlank)
        {                        
            Insert(editPages, isBlank);//            
            DeletePage(editPages);//刪除頁

            Document.SaveToFile(Path.Combine(Path.GetPathRoot(PDFPath), "123.pdf"));
            return GetPDFBase64();
        }

        /// <summary>
        /// PDF上依參數塞入印鑑
        /// </summary>
        /// <param name="editPages"></param>
        private void Insert(List<EditPage> editPages,bool isBlank)
        {
            for (int i = editPages.Count - 1; i > 0; i--)
            {
                if (!editPages[i].DeleteCheck)
                {
                    if(isBlank & editPages[i].BlankCheck)
                    {
                        InsertBlankPage(editPages[i].PageNumber);
                    }
                    InsertAccountantCertificate();//加入會計師證明書
                    foreach (EditImage editImage in editPages[i].EditImages)
                    {
                        Document.Pages[editPages[i].PageNumber].Canvas.DrawImage
                        (
                            PdfImage.FromStream(editImage.ImageStream),
                            editImage.Left,
                            editImage.Top,
                            editImage.Width,
                            editImage.Height
                        );
                    }
                }
            }
        }

        /// <summary>
        /// 刪除頁
        /// </summary>
        /// <param name="editPages"></param>
        private void DeletePage(List<EditPage> editPages)
        {
            foreach (EditPage editPage in editPages)
            {
                if (editPage.DeleteCheck)
                {
                    Document.Pages.RemoveAt(editPage.PageNumber);
                }
            }
        }

        private void InsertBlankPage(int pageNumber)
        {
            if (pageNumber % 2 == 0)
            {
                Document.Pages.Insert(pageNumber + 1);
                Document.Pages.Insert(pageNumber - 1);
            }
            else
            {
                Document.Pages.Insert(pageNumber + 1);
            }
        }



        private void InsertAccountantCertificate()
        {

        }


        private string GetMemoryStreamToBase64(MemoryStream memoryStream)
        {
            byte[] pdfBytes = memoryStream.ToArray();
            return $"{"data:application/pdf;base64,"}{Convert.ToBase64String(pdfBytes)}";
        }        
    }
}
