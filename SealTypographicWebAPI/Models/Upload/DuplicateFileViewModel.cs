namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 確認重複檔名
    /// </summary>
    public class DuplicateFileViewModel
    {
        /// <summary>
        /// 檔案ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 檔名
        /// </summary>
        public string FileName { get; set; }
    }
}
