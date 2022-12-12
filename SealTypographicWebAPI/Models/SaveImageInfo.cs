namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 存到SERVER端
    /// </summary>
    public class SaveImageInfo
    {
        /// <summary>
        /// 存檔位置
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// 檔名
        /// </summary>
        public string Filename { get; set; }

    }
}
