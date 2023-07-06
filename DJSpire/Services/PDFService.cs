using DJSpire.Consts;
using DJSpire.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SkiaSharp;
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

        //public PDFService()
        //{            
        //    Spire.License.LicenseProvider.SetLicenseFileFullPath($"{AppDomain.CurrentDomain.BaseDirectory}license.elic.xml");
        //}
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
            //int width = (int)(image.Width * scale);
            //int height = (int)(image.Height * scale);
            //image.Mutate(delegate (IImageProcessingContext x)
            //{
            //    x.Resize(width, height);
            //});
            return image.ToBase64String(format);            
        }

        /// <summary>
        /// PDF檔案轉Base64
        /// </summary>
        /// <returns></returns>
        public string GetPDFBase64(PDFColor pdfColor)
        {
            MemoryStream stream = new MemoryStream();            
            Document.SaveToStream(stream);
            if (pdfColor == PDFColor.GrayScale)
            {
                MemoryStream grayStream = new MemoryStream();
                PdfGrayConverter pdfGrayConverter = new PdfGrayConverter(stream);
                pdfGrayConverter.ToGrayPdf(grayStream);                
                return GetMemoryStreamToBase64(grayStream);
            }
            else
            {                
                return GetMemoryStreamToBase64(stream);
            }                                      
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
            return GetPDFBase64(editPDF.PDFColor);
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
                        //Create PdfUnitConvertor to convert the unit
                        PdfUnitConvertor unitCvtr = new PdfUnitConvertor();                        
                        Document.Pages[editPDF.EditPages[pageIndex].PageNumber].Canvas.SetTransparency(1f, 1f, PdfBlendMode.Multiply);                        
                        Document.Pages[editPDF.EditPages[pageIndex].PageNumber].Canvas.DrawImage
                        (
                            PdfImage.FromStream(editImage.ImageStream),
                            //1 inch = 72pt, and when dpi = 300, 1 inch = 300px. So when dpi = 300, 1px = 0.24pt
                            editImage.Left * 0.24f,
                            editImage.Top * 0.24f,
                            editImage.Width * 0.24f,
                            editImage.Height * 0.24f
                        );
                    }
                    if (editPDF.IsBlank & editPDF.EditPages[pageIndex].BlankCheck)//確認是否加入空白頁
                    {
                        InsertBlankPage(editPDF.EditPages[pageIndex].PageNumber + 1);//新增空白頁
                    }
                    if (editPDF.EditPages[pageIndex].AccountantCertificatePath != string.Empty)
                    {
                        Document.Pages.Insert(editPDF.EditPages[pageIndex].PageNumber);
                        PdfImage pdfImage = PdfImage.FromStream(editPDF.EditPages[pageIndex].AccountantCertificateImageStream);

                        Document.Pages[editPDF.EditPages[pageIndex].PageNumber].Canvas.DrawImage
                        (
                            pdfImage, 0, 0
                        );                        
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

        private string GetMemoryStreamToBase64(MemoryStream memoryStream)
        {
            byte[] pdfBytes = memoryStream.ToArray();
            return $"{"data:application/pdf;base64,"}{Convert.ToBase64String(pdfBytes)}";
        }        
    }
}
