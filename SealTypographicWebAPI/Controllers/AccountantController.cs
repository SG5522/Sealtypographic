using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Services;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師基本資料
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class AccountantController : ControllerBase
    {
        /// <summary>
        /// 會計師資料管理Service
        /// </summary>
        private readonly IAccountantService accountantService;



        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="accountantService">管理會計師資料</param>
        public AccountantController(IAccountantService accountantService)
        {
            this.accountantService = accountantService;
        }

        /// <summary>
        /// 依搜尋條件獲得資料列表(分頁)
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantPaginateViewModel Paginate([FromQuery]AccountantSearch accountantSearch)
        {
            AccountantPaginateViewModel accountantPaginatesViewModel = new();
            try
            {
                Log.Information("AccountantSignAuthorization get paginate input {@Input}", accountantSearch);
                accountantPaginatesViewModel = accountantService.GetPaginate(accountantSearch);
                Log.Information("AccountantSignAuthorization get paginate output {@Output}", accountantPaginatesViewModel);                
            }
            catch (Exception ex) 
            {
                Log.Error("AccountantSignAuthorization get paginate error {@Error}", ex.Message); 
                accountantPaginatesViewModel.DbError();                                
            }
            return accountantPaginatesViewModel;
        }

        /// <summary>
        /// 依搜尋條件獲得資料列表(排版使用)
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantPaginateViewModel PaginateWithTypographic([FromQuery] AccountantSearch accountantSearch)
        {
            AccountantPaginateViewModel accountantPaginatesViewModel = new();
            try
            {
                Log.Information("AccountantSignAuthorization paginateWithTypographic input {@Input}", accountantSearch);
                accountantPaginatesViewModel = accountantService.GetPaginateWithTypographic(accountantSearch);
                Log.Information("AccountantSignAuthorization paginateWithTypographic output {@Output}", accountantPaginatesViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignAuthorization paginateWithTypographic error {@Error}", ex.Message);
                accountantPaginatesViewModel.DbError();
            }
            return accountantPaginatesViewModel;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        /// <returns></returns>
        [HttpGet("{accountantId}")]
        public AccountantDetailResponse Detail(int accountantId)
        {
            AccountantDetailResponse accountantResponse = new();
            try
            {
                Log.Information("AccountantSignAuthorization get detail input {@Input}", accountantId);
                accountantResponse = accountantService.GetDetail(accountantId);
                Log.Information("AccountantSignAuthorization get detail output {@Output}", accountantResponse);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get detail error {@Error}", ex.Message); 
                accountantResponse.DbError();                                
            }
            return accountantResponse;
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="accountantForm">會計師資料</param>
        [HttpPost]
        public AccountantCreateResponse New(AccountantForm accountantForm)
        {
            AccountantCreateResponse accountantCreateResponse = new();
            try
            {
                Log.Information("AccountantSignAuthorization new input {@Input}", accountantForm);
                accountantCreateResponse = accountantService.New(accountantForm);
                Log.Information("AccountantSignAuthorization new output {@Output}", accountantCreateResponse);                       
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignAuthorization new error {@Error}", ex.Message); 
                accountantCreateResponse.DbError();                
            }
            return accountantCreateResponse;
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="accountantFormUpdate">會計師資料(Id為查詢用)</param>        
        [HttpPut]
        public ResponseViewModel Update(AccountantUpdateForm accountantFormUpdate)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSignAuthorization put input {@Input}", accountantFormUpdate);
                response = accountantService.Update(accountantFormUpdate);
                Log.Information("AccountantSignAuthorization put output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignAuthorization put accountantFormUpdate error {@Error}", ex.Message); 
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        [HttpDelete("{accountantId}")]
        public ResponseViewModel Delete(int accountantId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSignAuthorization delete(hide) input {@Input}", accountantId);
                response = accountantService.Delete(accountantId);
                Log.Information("AccountantSignAuthorization delete(hide) output {@Output}", response);
            }
            catch (Exception ex)
            { 
                Log.Error("AccountantSignAuthorization delete(hide) error {@Error}", ex.Message); 
                response.DbError();                
            }
            return response;
        }
    }
}
