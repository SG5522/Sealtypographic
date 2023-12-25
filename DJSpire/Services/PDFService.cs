using DJSpire.Consts;
using DJSpire.Models;
using Spire.Pdf;
using Spire.Pdf.Conversion;
using Spire.Pdf.Graphics;
using System;
using System.IO;


namespace DJSpire.Services
{
    public class PDFService
    {
        private string pdfPath;
        private int pageIndex;
        private const string DATA_URL = "data:application/pdf;base64,";

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
            get { return pdfPath; }
            set
            {
                pdfPath = value;
                if (!string.IsNullOrWhiteSpace(pdfPath))
                {
                    Document = new PdfDocument(pdfPath);
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

        public byte[] GetPDFBytes(PDFColor pdfColor)
        {
            MemoryStream stream = new MemoryStream();
            Document.SaveToStream(stream);
            if (pdfColor == PDFColor.GrayScale)
            {
                MemoryStream grayStream = new MemoryStream();
                PdfGrayConverter pdfGrayConverter = new PdfGrayConverter(stream);
                pdfGrayConverter.ToGrayPdf(grayStream);

                return grayStream.ToArray();
            }
            else
            {
                return stream.ToArray();
            }
        }

        /// <summary>
        /// PDF檔案轉Base64
        /// </summary>
        /// <returns></returns>
        public string GetPDFBase64(PDFColor pdfColor)
        {
            return $"{DATA_URL}{Convert.ToBase64String(GetPDFBytes(pdfColor))}";
        }

        /// <summary>
        /// PDF檔案轉Base64
        /// </summary>
        /// <returns></returns>
        //public string GetPDFBase64(PDFColor pdfColor)
        //{
        //    MemoryStream stream = new MemoryStream();            
        //    Document.SaveToStream(stream);
        //    if (pdfColor == PDFColor.GrayScale)
        //    {
        //        MemoryStream grayStream = new MemoryStream();
        //        PdfGrayConverter pdfGrayConverter = new PdfGrayConverter(stream);
        //        pdfGrayConverter.ToGrayPdf(grayStream);                
        //        return GetMemoryStreamToBase64(grayStream);
        //    }
        //    else
        //    {                
        //        return GetMemoryStreamToBase64(stream);
        //    }                                      
        //}

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
            return $"{DATA_URL}{Convert.ToBase64String(GetEditPDFBytes(editPDF))}";
        }

        /// <summary>
        /// 取得包含編輯內容的PDFBase64
        /// </summary>
        /// <param name="editPDF"></param>
        /// <returns></returns>
        public byte[] GetEditPDFBytes(EditPDF editPDF)
        {
            EditInsert(editPDF);
            return GetPDFBytes(editPDF.PDFColor);
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
                        PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(300f);                        
                        Document.Pages[editPDF.EditPages[pageIndex].PageNumber].Canvas.SetTransparency(1f, 1f, PdfBlendMode.Multiply);                        
                        Document.Pages[editPDF.EditPages[pageIndex].PageNumber].Canvas.DrawImage
                        (
                            PdfImage.FromStream(editImage.ImageStream),                                                    
                            //editImage.Left * editImage.ImageScale,
                            //editImage.Top * editImage.ImageScale,
                            //editImage.Width * editImage.ImageScale,
                            //editImage.Height * editImage.ImageScale
                            pdfUnitConvertor.ConvertToPixels(editImage.Left, PdfGraphicsUnit.Pixel),
                            pdfUnitConvertor.ConvertToPixels(editImage.Top, PdfGraphicsUnit.Pixel),
                            pdfUnitConvertor.ConvertToPixels(editImage.Width, PdfGraphicsUnit.Pixel),
                            pdfUnitConvertor.ConvertToPixels(editImage.Left, PdfGraphicsUnit.Pixel)
                        );
                    }
                    if (editPDF.IsBlank & editPDF.EditPages[pageIndex].BlankCheck)//確認是否加入空白頁
                    {
                        InsertBlankPage(editPDF.EditPages[pageIndex].PageNumber + 1);//新增空白頁
                    }
                    //插入會計師證明書
                    if (editPDF.EditPages[pageIndex].AccountantCertificatePath != string.Empty)
                    {                                                
                        if (Path.GetExtension(editPDF.EditPages[pageIndex].AccountantCertificatePath) != ".pdf")
                        {                            
                            PdfImage pdfImage = PdfImage.FromStream(editPDF.EditPages[pageIndex].AccountantCertificateStream);
                            //插入空白頁至指定頁數的下一頁
                            Document.Pages.Insert(editPDF.EditPages[pageIndex].PageNumber + 1);
                            Document.Pages[editPDF.EditPages[pageIndex].PageNumber +1].Canvas.DrawImage
                            (
                                pdfImage, 0, 0
                            );
                        }
                        else
                        {
                            PdfDocument accountantCertificatePdf = new PdfDocument(editPDF.EditPages[pageIndex].AccountantCertificateStream);                            
                            Document.InsertPage(accountantCertificatePdf, 0, editPDF.EditPages[pageIndex].PageNumber + 1);                            
                        }                      
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
            return $"{DATA_URL}{Convert.ToBase64String(pdfBytes)}";
        }        
    }
}
