using Microsoft.AspNetCore.Mvc;
using DBEntities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Services;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理信頭圖片
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class LetterheadImageController : ControllerBase
    {
        /// <summary>
        /// 管理信頭圖片service
        /// </summary>
        private readonly ILetterheadImageService letterheadImageService;

        /// <summary>
        /// 建構:注入管理信頭圖片service
        /// </summary>
        /// <param name="letterheadImageService">信頭圖片管理service</param>
        public LetterheadImageController(ILetterheadImageService letterheadImageService)
        {
            this.letterheadImageService = letterheadImageService;
        }

        /// <summary>
        /// 取得信頭圖案狀態列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public LetterheadImageStatusResponse StatusList()
        {
            LetterheadImageStatusResponse letterheadImageStatusResponse = new();
            try
            {                
                letterheadImageStatusResponse = letterheadImageService.GetStatus();
                Log.Information("LetterheadImage StatusList output {@Output}", letterheadImageStatusResponse);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage StatusList error {@Error}", ex);
                letterheadImageStatusResponse.DbError();
            }
            return letterheadImageStatusResponse;
        }

        /// <summary>
        /// 取得信頭名稱與圖片建立日期
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        [HttpGet("{letterheadId}")]
        public LetterheadImageCreateDateViews NameAndCreateDate(int letterheadId)
        {
            LetterheadImageCreateDateViews letterheadGroupCreateDateViews = new();
            try
            {
                Log.Information("LetterheadImage get createDates input {@Input}", letterheadId);
                letterheadGroupCreateDateViews = letterheadImageService.GetNameAndCreateDate(letterheadId);
                Log.Information("LetterheadImage get createDates output {@Output}", letterheadGroupCreateDateViews);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage get createDates error {@Error}", ex);
                letterheadGroupCreateDateViews.DbError();
            }
            return letterheadGroupCreateDateViews;
        }

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="id">信頭圖片Id</param>
        /// <returns></returns>
        [HttpGet]
        public LetterheadImageViewModel ImageViewModel(int id)
        {
            LetterheadImageViewModel letterheadImageViewModel = new();
            try
            {
                Log.Information("LetterheadImage get imageViewModel input {@Input}", id);
                letterheadImageViewModel = letterheadImageService.GetImageViewModel(id);
                Log.Information("LetterheadImage get imageViewModel output {@Output}", letterheadImageViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage get imageViewModel error {@Error}", ex);
                letterheadImageViewModel.DbError();
            }
            return letterheadImageViewModel;
        }

        /// <summary>
        /// 新增信頭圖片
        /// </summary>
        /// <param name="letterheadImageForm">信頭圖片</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel New(LetterheadImageForm letterheadImageForm)
        {
            ResponseViewModel responseViewModel = new();
            try
            {
                Log.Information("LetterheadImage new input {@Input}", letterheadImageForm);
                responseViewModel = letterheadImageService.New(letterheadImageForm);
                Log.Information("LetterheadImage new output {@Output}", responseViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage new error {@Error}", ex);
                responseViewModel.DbError();
            }
            return responseViewModel;
        }

        /// <summary>
        /// 異動信頭圖片
        /// </summary>
        /// <param name="letterheadImageUpdate">異動信頭圖片資料</param>
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel Update(LetterheadImageUpdate letterheadImageUpdate)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("LetterheadImage put input {@Input}", letterheadImageUpdate);
                response = letterheadImageService.Update(letterheadImageUpdate);
                Log.Information("LetterheadImage put output {@Output}", response);
            }            
            catch (Exception ex)
            {
                Log.Error("LetterheadImage put error {@Error}", ex);
                response.DbError();
            }
            return response;
        }        
    }
}
