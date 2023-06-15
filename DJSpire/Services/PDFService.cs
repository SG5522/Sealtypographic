using DJSpire.Consts;
using DJSpire.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using Spire.Pdf;
using Spire.Pdf.Conversion;
using Spire.Pdf.Graphics;
using System;
using System.IO;


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

        public string GetPageImageBase64(ImageType imageType = ImageType.Png)
        {
            Image image = Image.Load(GetPageImageStream(imageType), out IImageFormat format);
            return image.ToBase64String(format);            
        }

        /// <summary>
        /// PDF檔案轉Base64
        /// </summary>
        /// <returns></returns>
        public string GetPDFBase64(PdfColorSpace pdfColorSpace)
        {
            MemoryStream stream = new MemoryStream();
            Document.SaveToStream(stream);
            if (pdfColorSpace == PdfColorSpace.GrayScale)
            {
                PdfGrayConverter pdfGrayConverter = new PdfGrayConverter(stream);
                pdfGrayConverter.ToGrayPdf(stream);
            }
            FileStream streamToWrite = new FileStream(Path.Combine(@"D:\", "123.pdf"), FileMode.Create);            
            stream.CopyTo(streamToWrite);
            return GetMemoryStreamToBase64(stream);
        }

        public string GetPDFPageBase64()
        {
            MemoryStream stream = new MemoryStream();
            IndexDocument.SaveToStream(stream);
            return GetMemoryStreamToBase64(stream);
        }

        /// <summary>
        /// 回傳PDF總頁數
        /// </summary>
        /// <returns></returns>
        public int GetTotalPage()
        {
            return Document.Pages.Count;
        }

        /// <summary>
        /// 取得包含編輯內容的PDFBase64
        /// </summary>
        /// <param name="editPDF"></param>
        /// <returns></returns>
        public string GetEditPDFBase64(EditPDF editPDF)
        {            
            EditInsert(editPDF);                          
            return GetPDFBase64(editPDF.PdfColorSpace);
        }

        /// <summary>
        /// PDF上依參數塞入印鑑
        /// </summary>
        /// <param name="editPDF"></param>
        private void EditInsert(EditPDF editPDF)
        {
            for (int pageCount = editPDF.EditPages.Count ; pageCount > 0; pageCount--)
            {
                int pageIndex = pageCount - 1;
                if (!editPDF.EditPages[pageIndex].DeleteCheck)//確認此頁是否為刪除
                {
                    foreach (EditImage editImage in editPDF.EditPages[pageIndex].EditImages)
                    {
                        Document.Pages[editPDF.EditPages[pageIndex].PageNumber].Canvas.DrawImage
                        (
                            PdfImage.FromStream(editImage.ImageStream),
                            editImage.Left,
                            editImage.Top,
                            editImage.Width,
                            editImage.Height
                        );
                    }
                    if (editPDF.IsBlank & editPDF.EditPages[pageIndex].BlankCheck)//確認是否加入空白頁
                    {
                        InsertBlankPage(editPDF.EditPages[pageIndex].PageNumber + 1);//新增空白頁
                    }
                    if (editPDF.EditPages[pageIndex].IsAccountantCertificate)
                    {
                        InsertAccountantCertificate(editPDF.EditPages[pageIndex].PageNumber);//加入會計師證明書
                    }                    
                }
                else
                {
                    Document.Pages.RemoveAt(editPDF.EditPages[pageIndex].PageNumber);
                }
            }
        }


        private void InsertBlankPage(int pageNumber)
        {            
            if (pageNumber % 2 == 0)
            {
                Document.Pages.Insert(pageNumber - 1);
                Document.Pages.Insert(pageNumber + 1);                
            }
            else
            {
                Document.Pages.Insert(pageNumber);
            }
        }



        private void InsertAccountantCertificate(int pageNumber)
        {
            Document.Pages.Insert(pageNumber);
        }


        private string GetMemoryStreamToBase64(MemoryStream memoryStream)
        {
            byte[] pdfBytes = memoryStream.ToArray();
            return $"{"data:application/pdf;base64,"}{Convert.ToBase64String(pdfBytes)}";
        }        
    }
}
