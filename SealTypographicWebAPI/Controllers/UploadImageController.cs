using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using SealTypographicWebAPI.Util;
using Serilog;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 上傳
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]    
    public class UploadImageController : ControllerBase
    {
        private readonly UploadService uploadService;
        private readonly ImageService imageService;

        /// <summary>
        /// 注入UploadService
        /// </summary>
        /// <param name="uploadService"></param>
        /// <param name="imageService"></param>        
        public UploadImageController(UploadService uploadService, ImageService imageService)
        {
            this.uploadService = uploadService;            
            this.imageService = imageService;
        }

        /// <summary>
        /// 上傳圖檔
        /// </summary>
        /// <param name="uploadScanForms">圖檔資料</param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadScanForms")]        
        public ResponseViewModel PostImagebase64(List<UploadScanForm> uploadScanForms)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("UploadImage post uploadScanForms input {@Input}", uploadScanForms);                
                //ResponseViewModel response = uploadService.SaveImageBase64(uploadScanForms);

                Log.Information("UploadImage post uploadScanForms output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("UploadImage Post error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }

        /// <summary>
        /// 上傳圖檔
        /// </summary>
        /// <param name="formFiles">圖檔資料</param>
        /// <param name="uploadType" example="1">上傳類別 1.客戶印鑑授權書 2.會計印鑑簽名授權書 3.信頭</param>        
        /// <returns></returns>
        [HttpPost]
        [Route("uploadIFormFiles")]
        public async Task<ResponseViewModel> PostImage([FromForm]List<IFormFile> formFiles, int uploadType)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("UploadImage post uploadScanForms input {@Input}", formFiles);                        
                response = await uploadService.SaveImageIFormFile(uploadType, formFiles);
                Log.Information("UploadImage post uploadScanForms output {@Output}", response);                       
            }
            catch (Exception ex)
            {
                Log.Error("UploadImage Post error {@Error}", ex);
                response.DbError();
            }
            return response;
        }
    }
}
