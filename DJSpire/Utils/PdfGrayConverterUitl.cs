using Spire.Pdf;
using Spire.Pdf.Conversion;


namespace SealTypographicWebAPI.Utils.Pdf
{
    public static partial class PdfUitl
    {
        /// <summary>
        /// pdf灰階處理 (Input FilePath)
        /// </summary>
        /// <param name="srcPath">FilePath</param>
        /// <returns></returns>
        public static byte[] PdfGrayConverter(string srcPath)
        {
            return PdfGrayConverter(File.ReadAllBytes(srcPath));
        }

        /// <summary>
        /// pdf灰階處理 (Input Bytes)
        /// </summary>
        /// <param name="srcBytes">Bytes</param>
        /// <returns></returns>
        public static byte[] PdfGrayConverter(byte[] srcBytes)
        {
            return PdfGrayConverter(new PdfDocument(srcBytes));
        }

        /// <summary>
        /// pdf灰階處理 (Input FilePath)
        /// </summary>
        /// <param name="srcPath">FilePath</param>
        /// <returns></returns>
        public static string PdfGrayConverterToDataURL(string srcPath)
        {
            return PdfGrayConverterToDataURL(File.ReadAllBytes(srcPath));
        }

        /// <summary>
        /// pdf灰階處理 (Input Bytes)
        /// </summary>
        /// <param name="srcBytes">Bytes</param>
        /// <returns></returns>
        public static string PdfGrayConverterToDataURL(byte[] srcBytes)
        {
            return PdfGrayConverterToDataURL(new PdfDocument(srcBytes));
        }

        /// <summary>
        /// pdf灰階處理 (Input PdfDocument)
        /// </summary>
        /// <param name="pdfDocument">Spire Pdf Document Class</param>
        /// <returns></returns>
        public static string PdfGrayConverterToDataURL(PdfDocument pdfDocument)
        {
            return $"{DATA_URL}{Convert.ToBase64String(PdfGrayConverter(pdfDocument))}";
        }

        /// <summary>
        /// pdf灰階處理 (Input PdfDocument)
        /// </summary>
        /// <param name="pdfDocument">Spire Pdf Document Class</param>        
        /// <returns></returns>
        public static byte[] PdfGrayConverter(PdfDocument pdfDocument)
        {
            MemoryStream stream = new();
            MemoryStream grayStream = new();
            pdfDocument.SaveToStream(stream);            
            PdfGrayConverter pdfGrayConverter = new(stream);
            pdfGrayConverter.ToGrayPdf(grayStream);
            return grayStream.ToArray();
        }
    }
}
