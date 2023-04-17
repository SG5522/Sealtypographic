using DBEntities.Consts;
using DBEntities;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// IFormFile相關的處理。
    /// </summary>
    public class IFormFileUtil
    {
        /// <summary>
        /// 透過IFromFile存檔
        /// </summary>
        /// <param name="formFile">IFormFile</param>
        /// <param name="savePath">存檔路徑</param>
        /// <returns></returns>
        public static async Task UploadFileReturnPath(IFormFile formFile, string savePath)
        {            
            using Stream stream = new FileStream(savePath, FileMode.Create);
            await formFile.CopyToAsync(stream);            
        }
    }
}
