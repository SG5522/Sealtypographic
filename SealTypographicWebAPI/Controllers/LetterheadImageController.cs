using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Services;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 信頭資料處理
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LetterheadImageController : ControllerBase
    {
        /// <summary>
        /// 宣告信頭的interface
        /// </summary>
        protected readonly ILetterheadImageService letterheadImageService;

        /// <summary>
        /// 注入信頭interface
        /// </summary>
        /// <param name="letterheadImageService"></param>
        public LetterheadImageController(ILetterheadImageService letterheadImageService)
        {
            this.letterheadImageService = letterheadImageService;
        }

        /// <summary>
        /// 取得信頭
        /// </summary>
        /// <param name="letterheadId"></param>
        /// <returns></returns>
        [HttpGet("{letterheadId}")]
        public LetterheadGroupCreateDateViews Get(int letterheadId)
        {
            LetterheadGroupCreateDateViews letterheadGroupCreateDateViews = new();
            try
            {
                Log.Information("LetterheadImage get{letterheadId} input {@Input}", letterheadId);
                letterheadGroupCreateDateViews = letterheadImageService.GetLetterheadCreateDateViews(letterheadId);
                Log.Information("LetterheadImage get{letterheadId} output {@Output}", letterheadGroupCreateDateViews);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage get{letterheadId} error {@Error}", ex);
                letterheadGroupCreateDateViews.DbError();
            }
            return letterheadGroupCreateDateViews;
        }

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch">搜尋條件</param>
        /// <returns></returns>
        [HttpGet]
        public LetterheadImageViewModels Get([FromQuery]LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch)
        {
            LetterheadImageViewModels letterheadImageViewModels = new();
            try
            {
                Log.Information("LetterheadImage get input {@Input}", letterheadImageGroupCreateDateSearch);
                letterheadImageViewModels = letterheadImageService.GetLetterheadImages(letterheadImageGroupCreateDateSearch);
                Log.Information("LetterheadImage get output {@Output}", letterheadImageViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage get error {@Error}", ex);
                letterheadImageViewModels.DbError();
            }
            return letterheadImageViewModels;
        }

        /// <summary>
        /// 新增信頭圖片組
        /// </summary>
        /// <param name="letterheadImageForms">信頭圖片組</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel Post(LetterheadImageForms letterheadImageForms)
        {
            ResponseViewModel responseViewModel = new();
            try
            {
                Log.Information("LetterheadImage post input {@Input}", letterheadImageForms);
                responseViewModel = letterheadImageService.Create(letterheadImageForms);
                Log.Information("LetterheadImage post output {@Output}", responseViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage post error {@Error}", ex);
                responseViewModel.DbError();
            }
            return responseViewModel;
        }

        /// <summary>
        /// 修改信頭圖片組
        /// </summary>
        /// <param name="letterheadImageUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        [HttpPut]
        public List<ResponseViewModel> Put(LetterheadImageUpdate letterheadImageUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();
            try
            {
                Log.Information("LetterheadImage put input {@Input}", letterheadImageUpdate);
                responseViewModels = letterheadImageService.Update(letterheadImageUpdate);
                Log.Information("LetterheadImage put output {@Output}", responseViewModels);
            }            
            catch (Exception ex)
            {
                ResponseViewModel responseViewModel = new();
                Log.Error("LetterheadImage put error {@Error}", ex);                
                responseViewModel.DbError();
                responseViewModels.Add(responseViewModel);
            }
            return responseViewModels;
        }

        /// <summary>
        /// 變更此群組創建日期的信頭圖片審核為通過(啟用)
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch"></param>        
        /// <returns></returns>
        [HttpPut("Approval")]
        public ResponseViewModel PutApprovalLetterheadImage(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("LetterheadImage put(Approval) input {@Input}", letterheadImageGroupCreateDateSearch);
                response = letterheadImageService.ApprovalLetterheadImages(letterheadImageGroupCreateDateSearch);
                Log.Information("LetterheadImage put(Approval) output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage put(Approval) error {@Error}", ex);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 變更此群組創建日期的信頭圖片審核為作廢
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch"></param>
        /// <returns></returns>
        [HttpPut("Invalid")]
        public ResponseViewModel PutInvalidLetterheadImage(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("LetterheadImage put(Invalid) input {@Input}", letterheadImageGroupCreateDateSearch);
                response = letterheadImageService.InvalidLetterheadImages(letterheadImageGroupCreateDateSearch);
                Log.Information("LetterheadImage put(Invalid) output {@Ouput}", response);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage put(Invalid) error {@Error}", ex);
                response.DbError();
            }
            return response;
        }
    }
}
