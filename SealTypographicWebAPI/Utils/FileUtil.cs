
using Microsoft.AspNetCore.Http;
using System;
using System.Text.RegularExpressions;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// IFormFile相關的處理。
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 取得依系統時間組成另外組成檔案路徑及檔案名稱
        /// </summary>
        /// <param name="rootFolder">存檔根目錄</param>
        /// <param name="fileName">檔名</param>
        /// <returns></returns>
        public static string GetSaveFullPathWithDateTime(string rootFolder, string fileName = "")
        {
            DateTime dateTime = DateTime.Now;            
            string dateFolder = Path.Combine
                                (
                                    dateTime.Year.ToString(),
                                    dateTime.Month.ToString(),
                                    dateTime.Day.ToString()
                                );            
            fileName = $"{Path.GetFileNameWithoutExtension(fileName)}{dateTime:yyyyMMHHmmssffff}{Path.GetExtension(fileName)}";
            CheckDirectory(Path.Combine(rootFolder, dateFolder));

            return Path.Combine(rootFolder, dateFolder, fileName); ;
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
        /// 確認是否有圖檔，如有就刪除
        /// </summary>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static void DeleteFile(string savePath)
        {
            if(File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }
    }
}
