
namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// IFormFile相關的處理。
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 透過IFromFile存檔並回傳存檔位置
        /// </summary>
        /// <param name="formFile">IFormFile</param>
        /// <param name="code">公司編號</param>
        /// <param name="rootFolder">存檔路徑</param>
        /// <returns></returns>
        public static async Task<string> UploadFileReturnPath(IFormFile formFile, string code, string rootFolder)
        {
            DateTime dateTime = DateTime.Now;
            string fileName = $"{code}{dateTime:yyyyMMHHmmssffff}{Path.GetExtension(formFile.FileName)}";            
            string dateFolder = Path.Combine
                                (
                                    dateTime.Year.ToString(),
                                    dateTime.Month.ToString(),
                                    dateTime.Day.ToString()
                                );
            string savePath = Path.Combine(rootFolder, dateFolder, fileName);
            CheckDirectory(Path.Combine(rootFolder, dateFolder));            
            await SaveUpdata(formFile, savePath);

            return savePath;
        }

        /// <summary>
        /// 確認是否有資料夾沒有則先建立
        /// </summary>
        /// <param name="savePathRoot"></param>
        public static void CheckDirectory(string savePathRoot)
        {
            if (!Directory.Exists(savePathRoot))
            {
                Directory.CreateDirectory(savePathRoot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="savePath"></param>
        public static async Task SaveUpdata(IFormFile formFile, string savePath)
        {
            using Stream stream = new FileStream(savePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
        }

        /// <summary>
        /// 確認是否有圖檔，如有就刪除
        /// </summary>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static void DeleteImage(string savePath)
        {
            if(File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }
    }
}
