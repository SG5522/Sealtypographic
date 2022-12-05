namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 資料夾
    /// </summary>
    public class FolderUtil
    {
        /// <summary>
        /// 確認SERVER本機是否有資料夾路徑，若無則建立。
        /// </summary>
        /// <param name="folderPath">資料夾路徑</param>
        public static void CheckFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
