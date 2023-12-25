using SealTypographicWebAPI.Models.EditPdf;
using Spire.Pdf;
using Spire.Pdf.Graphics;


namespace SealTypographicWebAPI.Utils.Pdf
{
    public static partial class PdfUitl
    {
        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input FilePath)
        /// </summary>
        /// <param name="srcPath">FilePath</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static byte[] EditPdfToBytes(string srcPath, EditPDF editPDF, float dpi = 300f)
        {                        
            return EditPdfToBytes(File.ReadAllBytes(srcPath), editPDF, dpi);
        }

        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input Bytes)
        /// </summary>
        /// <param name="srcbytes">Bytes</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static byte[] EditPdfToBytes(byte[] srcbytes, EditPDF editPDF, float dpi = 300f)
        {                        
            return EditPdfToBytes(new PdfDocument(srcbytes), editPDF, dpi);
        }

        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input FilePath)
        /// </summary>
        /// <param name="srcPath">FilePath</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static string EditPdfToDataURL(string srcPath, EditPDF editPDF, float dpi = 300f)
        {
            return EditPdfToDataURL(File.ReadAllBytes(srcPath), editPDF, dpi);
        }

        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input Bytes)
        /// </summary>
        /// <param name="srcbytes">Bytes</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static string EditPdfToDataURL(byte[] srcbytes, EditPDF editPDF, float dpi = 300f)
        {
            return EditPdfToDataURL(new PdfDocument(srcbytes), editPDF, dpi);
        }

        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input FilePath)
        /// </summary>
        /// <param name="srcPath">FilePath</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static PdfDocument EditPdf(string srcPath, EditPDF editPDF, float dpi = 300f)
        {
            return EditPdf(File.ReadAllBytes(srcPath), editPDF, dpi);
        }

        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input Bytes)
        /// </summary>
        /// <param name="srcbytes">Bytes</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static PdfDocument EditPdf(byte[] srcbytes, EditPDF editPDF, float dpi = 300f)
        {
            return EditPdf(new PdfDocument(srcbytes), editPDF, dpi);
        }



        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input PdfDocument)
        /// </summary>
        /// <param name="pdfDocument">Spire Pdf Document Class</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static string EditPdfToDataURL(PdfDocument pdfDocument, EditPDF editPDF, float dpi = 300f)
        {
            return $"{DATA_URL}{Convert.ToBase64String(EditPdfToBytes(pdfDocument, editPDF, dpi))}";
        }

        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input PdfDocument)
        /// </summary>
        /// <param name="pdfDocument">Spire Pdf Document Class</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static byte[] EditPdfToBytes(PdfDocument pdfDocument, EditPDF editPDF, float dpi = 300f)
        {
            MemoryStream stream = new MemoryStream();
            EditPdf(pdfDocument, editPDF, dpi).SaveToStream(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// 參照EditPDF class塞入印鑑 (Input PdfDocument)
        /// </summary>
        /// <param name="pdfDocument">Spire Pdf Document Class</param>
        /// <param name="editPDF"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static PdfDocument EditPdf(PdfDocument pdfDocument, EditPDF editPDF, float dpi = 300f)
        {
            for (int pageCount = editPDF.EditPages.Count; pageCount > 0; pageCount--)
            {
                int pageIndex = pageCount - 1;
                if (!editPDF.EditPages[pageIndex].DeleteCheck)//確認此頁是否為刪除
                {
                    foreach (EditImage editImage in editPDF.EditPages[pageIndex].EditImages)
                    {
                        //Create PdfUnitConvertor to convert the unit
                        PdfUnitConvertor pdfUnitConvertor = new();
                        pdfDocument.Pages[editPDF.EditPages[pageIndex].PageNumber].Canvas.SetTransparency(1f, 1f, PdfBlendMode.Multiply);
                        pdfDocument.Pages[editPDF.EditPages[pageIndex].PageNumber].Canvas.DrawImage
                        (
                            PdfImage.FromStream(editImage.ImageStream),
                            editImage.Left * editImage.ImageScale,
                            editImage.Top * editImage.ImageScale,
                            editImage.Width * editImage.ImageScale,
                            editImage.Height * editImage.ImageScale
                        //pdfUnitConvertor.ConvertToPixels(editImage.Left, PdfGraphicsUnit.Pixel),
                        //pdfUnitConvertor.ConvertToPixels(editImage.Top, PdfGraphicsUnit.Pixel),
                        //pdfUnitConvertor.ConvertToPixels(editImage.Width, PdfGraphicsUnit.Pixel),
                        //pdfUnitConvertor.ConvertToPixels(editImage.Height, PdfGraphicsUnit.Pixel)
                        );
                    }
                    if (editPDF.IsBlank & editPDF.EditPages[pageIndex].BlankCheck)//確認是否加入空白頁
                    {
                        InsertBlankPage(pdfDocument, editPDF.EditPages[pageIndex].PageNumber + 1);//新增空白頁
                    }
                    //插入會計師證明書
                    if (editPDF.EditPages[pageIndex].AccountantCertificatePath != string.Empty)
                    {
                        if (Path.GetExtension(editPDF.EditPages[pageIndex].AccountantCertificatePath) != ".pdf")
                        {
                            PdfImage pdfImage = PdfImage.FromStream(editPDF.EditPages[pageIndex].AccountantCertificateStream);
                            //插入空白頁至指定頁數的下一頁
                            pdfDocument.Pages.Insert(editPDF.EditPages[pageIndex].PageNumber + 1);
                            pdfDocument.Pages[editPDF.EditPages[pageIndex].PageNumber + 1].Canvas.DrawImage
                            (
                                pdfImage, 0, 0
                            );
                        }
                        else
                        {
                            PdfDocument accountantCertificatePdf = new PdfDocument(editPDF.EditPages[pageIndex].AccountantCertificateStream);
                            pdfDocument.InsertPage(accountantCertificatePdf, 0, editPDF.EditPages[pageIndex].PageNumber + 1);
                        }
                    }
                }
                else
                {
                    pdfDocument.Pages.RemoveAt(editPDF.EditPages[pageIndex].PageNumber);
                }
            }

            return pdfDocument;
        }

        private static void InsertBlankPage(PdfDocument pdfDocument, int pageNumber)
        {
            if (pageNumber % 2 == 0)
            {
                pdfDocument.Pages.Insert(pageNumber - 1);
                pdfDocument.Pages.Insert(pageNumber + 1);
            }
            else
            {
                pdfDocument.Pages.Insert(pageNumber);
            }
        }
    }
}
