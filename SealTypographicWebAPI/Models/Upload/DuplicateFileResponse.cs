namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 確認重複檔名的檔案列
    /// </summary>
    public class DuplicateFileResponse : ResponseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public DuplicateFileResponse ()
        {
            ViewModel = new();
        }

        /// <summary>
        /// 檔名
        /// </summary>
        public List<DuplicateFileViewModel> ViewModel { get; set; }
    }
}
