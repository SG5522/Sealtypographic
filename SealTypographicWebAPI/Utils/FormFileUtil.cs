using DBEntities.Consts;
using DBEntities;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// IFormFile相關的處理。
    /// </summary>
    public class FormFileUtil
    {
        /// <summary>
        /// 透過IFromFile存檔並回傳存檔位置
        /// </summary>
        /// <param name="formFile">IFormFile</param>
        /// <param name="code">公司編號</param>
        /// <param name="rootFolder">存檔路徑</param>
        /// <returns></returns>
        public static async Task<string> UploadFileReturnPath(IFormFile formFile, string code,  string rootFolder)
        {
            DateTime dateTime = DateTime.Now;
            string fileName = $"{code}{dateTime:yyyyMMHHmmssffff}{Path.GetExtension(formFile.FileName)}";            
            string dateFolder = Path.Combine
                                (
                                    dateTime.Year.ToString(),
                                    dateTime.Month.ToString(),
                                    dateTime.Day.ToString()
                                );            
            if (!Directory.Exists(Path.Combine(rootFolder, dateFolder)))
            {
                Directory.CreateDirectory(Path.Combine(rootFolder, dateFolder));
            }

            string savePath = Path.Combine(rootFolder, dateFolder, fileName);

            await SaveUpdata(formFile, savePath);

            //using Stream stream = new FileStream(savePath, FileMode.Create);
            //await formFile.CopyToAsync(stream);

            return savePath;
        }

        /// <summary>
        /// 透過IFromFile存檔覆蓋檔案
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static async Task UploadFileReturnPath(IFormFile formFile, string savePath)
        {
            await SaveUpdata(formFile, savePath);            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="savePath"></param>
        private static async Task SaveUpdata(IFormFile formFile, string savePath)
        {
            string? savePathRoot = Path.GetPathRoot(savePath);

            if (!Directory.Exists(Path.GetPathRoot(savePath)))
            {
                Directory.CreateDirectory(Path.GetPathRoot(savePath));
            }

            using Stream stream = new FileStream(savePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
        }
    }
}
