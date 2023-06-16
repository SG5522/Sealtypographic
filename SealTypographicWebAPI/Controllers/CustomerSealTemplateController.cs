using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Services;
using Serilog;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 客戶印鑑樣板管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSealTemplateController : ControllerBase
    {
        private readonly ICustomerSealTemplateService customerSealTemplateService;

        /// <summary>
        /// 建構 注入Service
        /// </summary>
        /// <param name="customerSealTemplateService"></param>
        public CustomerSealTemplateController(ICustomerSealTemplateService customerSealTemplateService)
        {
            this.customerSealTemplateService = customerSealTemplateService;
        }

        /// <summary>
        /// 客戶印鑑樣板詳細資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public CustomerSealTemplateDetailViewModel Detail(int id)
        {
            CustomerSealTemplateDetailViewModel customerSealTemplateDetailViewModel = new();
            try
            {
                Log.Information("CustomerSealTemplate detail input {@Input}", id);
                customerSealTemplateDetailViewModel = customerSealTemplateService.GetDetail(id);
                Log.Information("CustomerSealTemplate detail output {@Output}", customerSealTemplateDetailViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate paginate error {@Error}", ex.Message); 
                customerSealTemplateDetailViewModel.DbError();
            }
            return customerSealTemplateDetailViewModel;
        }

        /// <summary>
        /// 客戶印鑑樣板圖片顯示
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]/{id}")]
        public CustomerSealTemplateImageView ViewImage(int id)
        {
            CustomerSealTemplateImageView viewImage = new();
            try
            {
                Log.Information("CustomerSealTemplate detail input {@Input}", id);
                viewImage = customerSealTemplateService.GetImage(id);
                Log.Information("CustomerSealTemplate detail output {@Output}", id);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate paginate error {@Error}", ex.Message); 
                viewImage.DbError();
            }
            return viewImage;
        }

        /// <summary>
        /// 客戶印鑑樣板分頁列表
        /// </summary>
        /// <param name="customerSealTemplateSearch">客戶印鑑樣板分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public CustomerSealTemplatePaginate Paginate([FromQuery]CustomerSealTemplateSearch customerSealTemplateSearch)
        {
            CustomerSealTemplatePaginate customerSealTemplatePaginate = new ();
            try
            {
                Log.Information("CustomerSealTemplate paginate input {@Input}", customerSealTemplateSearch);
                customerSealTemplatePaginate = customerSealTemplateService.GetPaginate(customerSealTemplateSearch);               
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate paginate error {@Error}", ex.Message); 
                customerSealTemplatePaginate.DbError();
            }
            return customerSealTemplatePaginate;
        }

        /// <summary>
        /// 新增客戶印鑑樣板
        /// </summary>
        /// <param name="customerSealTemplateForm"></param>        
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(CustomerSealTemplateForm customerSealTemplateForm)
        {
            ResponseViewModel response = new ();
            try
            {                
                Log.Information("CustomerSealTemplate new input {@Input}", $"{customerSealTemplateForm.Name}");
                response = await customerSealTemplateService.New(customerSealTemplateForm);
                Log.Information("CustomerSealTemplate new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate new error {@Error}", ex.Message); 
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 更新客戶印鑑樣板
        /// </summary>
        /// <param name="customerSealTemplateUpdateForm">客戶樣板</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(CustomerSealTemplateUpdateForm customerSealTemplateUpdateForm)
        {
            ResponseViewModel response = new();
            try
            {                
                Log.Information("CustomerSealTemplate update input {@input}", customerSealTemplateUpdateForm);
                response = await customerSealTemplateService.Update(customerSealTemplateUpdateForm);
                Log.Information("CustomerSealTemplate update output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate update error {@Error}", ex.Message); 
                response.DbError();
            }

            return response;
        }

        /// <summary>
        /// 刪除客戶印鑑樣板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ResponseViewModel Delete(int id)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealTemplate delete input  {@id}", id);
                response = customerSealTemplateService.Delete(id);
                Log.Information("CustomerSealTemplate delete output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate new error {@Error}", ex.Message); 
                response.DbError();
            }

            return response;
        }
    }
}
