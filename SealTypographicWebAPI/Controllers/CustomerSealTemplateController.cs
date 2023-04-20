using Azure;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        /// 
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
                Log.Information("CustomerSealTemplate detail output {@Output}", id);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate paginate error {@Error}", ex);
                customerSealTemplateDetailViewModel.DbError();
            }
            return customerSealTemplateDetailViewModel;
        }

        /// <summary>
        /// 依搜尋結果與分頁顯示樣板列表
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
                Log.Error("CustomerSealTemplate paginate error {@Error}", ex);
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
        public async Task<ResponseViewModel> New([FromForm]CustomerSealTemplateForm customerSealTemplateForm)
        {
            ResponseViewModel response = new ();
            try
            {                
                Log.Information("CustomerSealTemplate new input {@Input}", customerSealTemplateForm);
                response = await customerSealTemplateService.New(customerSealTemplateForm);
                Log.Information("CustomerSealTemplate new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate new error {@Error}", ex);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 更新客戶印鑑樣板
        /// </summary>
        /// <param name="id">樣板ID</param>
        /// <param name="customerSealTemplateUpdateForm">客戶樣板</param>
        [HttpPut("{id}")]
        public async Task<ResponseViewModel> Update([FromForm] CustomerSealTemplateUpdateForm customerSealTemplateUpdateForm)
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
                Log.Error("CustomerSealTemplate update error {@Error}", ex);
                response.DbError();
            }

            return response;
        }

        /// <summary>
        /// 
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
                //response = await customerSealTemplateService.Update(customerSealTemplateUpdateForm);
                Log.Information("CustomerSealTemplate delete output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate new error {@Error}", ex);
                response.DbError();
            }

            return response;
        }
    }
}
