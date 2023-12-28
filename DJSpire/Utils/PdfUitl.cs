using Spire.Pdf;


namespace SealTypographicWebAPI.Utils.Pdf
{
    /// <summary>
    /// SpirePdf的各項處理
    /// </summary>
    public static partial class PdfUitl
    {
        private const string DATA_URL = "data:application/pdf;base64,";

        /// <summary>
        /// 輸出DataURL
        /// </summary>
        /// <param name="srcPath"></param>
        /// <returns></returns>
        public static string ToDataURL(string srcPath)
        {
            return ToDataURL(File.ReadAllBytes(srcPath));
        }

        /// <summary>
        /// 輸出DataURL
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <returns></returns>
        public static string ToDataURL(byte[] srcBytes)
        {
            return ToDataURL(new PdfDocument(srcBytes));
        }

        /// <summary>
        /// 輸出Bytes
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <returns></returns>
        public static byte[] ToBytes(string srcPath)
        {
            return ToBytes(File.ReadAllBytes(srcPath));
        }

        /// <summary>
        /// 輸出Bytes
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <returns></returns>
        public static byte[] ToBytes(byte[] srcBytes)
        {
            return ToBytes(new PdfDocument(srcBytes));
        }

        /// <summary>
        /// 輸出DataURL
        /// </summary>
        /// <param name="pdfDocument"></param>
        /// <returns></returns>
        public static string ToDataURL(PdfDocument pdfDocument)
        {            
            return $"{DATA_URL}{Convert.ToBase64String(ToBytes(pdfDocument))}";
        }

        /// <summary>
        /// 輸出Bytes
        /// </summary>
        /// <param name="pdfDocument"></param>
        /// <returns></returns>
        public static byte[] ToBytes(PdfDocument pdfDocument)
        {
            MemoryStream memoryStream = new();
            pdfDocument.SaveToStream(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
