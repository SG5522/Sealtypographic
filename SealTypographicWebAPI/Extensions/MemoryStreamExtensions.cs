namespace SealTypographicWebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MemoryStreamExtensions
    {
        /// <summary>
        /// 將memoryStream放到起始位置在轉成Base64
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns>回傳base64</returns>
        public static string ToBase64String(this MemoryStream memoryStream)
        {
            // 確保 MemoryStream 的位置在起始位置
            memoryStream.Seek(0, SeekOrigin.Begin);
            return Convert.ToBase64String(memoryStream.ToArray());
        }
    }

}
