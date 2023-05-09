using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Services;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師簽印
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class AccountantSignController : ControllerBase
    {
        /// <summary>
        /// 會計師簽印管理Service
        /// </summary>
        private readonly IAccountantSignService accountantSignService;

        /// <summary>
        /// 建構：注入會計師簽印管理Service
        /// </summary>
        /// <param name="accountantSignService">會計師簽印管理Service</param>        
        public AccountantSignController(IAccountantSignService accountantSignService)
        {
            this.accountantSignService = accountantSignService;            
        }

        /// <summary>
        /// 取得會計師簽印建立日期列表
        /// </summary>
        /// <param name="accountantID">會計師ID</param>        
        /// <returns></returns>
        [HttpGet("{accountantID}")]
        public AccountantSignGroupResponse CreateDates(int accountantID)
        {
            AccountantSignGroupResponse accountantSignStartDates = new ();
            try
            {
                Log.Information("AccountantSign get CreateDates input {@Input}", accountantID);
                accountantSignStartDates = accountantSignService.GetCreateDates(accountantID);
                Log.Information("AccountantSign get CreateDates output {@Output}", accountantSignStartDates);                        
            }
            catch (Exception ex)
            {             
                Log.Error("AccountantSign get CreateDates error {@Error}", ex.Message); 
                accountantSignStartDates.DbError();                
            }
            return accountantSignStartDates;
        }

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>        
        /// <returns></returns>        
        [HttpGet]
        public AccountantSignViewModels Signs(int accountantSignGroupId)
        {
            AccountantSignViewModels accountantSignViewModels = new();
            try
            {
                Log.Information("AccountantSign get signViewModels input {@Input}", accountantSignGroupId);
                accountantSignViewModels = accountantSignService.GetSignViewModels(accountantSignGroupId);
                Log.Information("AccountantSign get signViewModels output {@Output}", accountantSignViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get signViewModels error {@Error}", ex.Message); 
                accountantSignViewModels.DbError();                
            }
            return accountantSignViewModels;
        }

        /// <summary>
        /// 新增會計師簽印組
        /// </summary>
        /// <param name="accountantSignForms">會計師簽印組</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(AccountantSignForms accountantSignForms)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantSign new input {@Input}", accountantSignForms);
                response = await accountantSignService.New(accountantSignForms);
                Log.Information("AccountantSign new output {@Output}", response);                             
            }
            catch (Exception ex)
            {                
                Log.Error("AccountantSign new error {@Error}", ex.Message); 
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 異動會計師簽印
        /// </summary>
        /// <param name="accountantSignUpdate">需要異動會計師簽印資料</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<List<ResponseViewModel>> Update(AccountantSignUpdate accountantSignUpdate)
        {
            List<ResponseViewModel> responses= new();
            try
            {
                Log.Information("AccountantSign update input {@Input}", accountantSignUpdate);
                responses = await accountantSignService.Update(accountantSignUpdate);
                Log.Information("AccountantSign update output {@Output}", responses);
            }
            catch (Exception ex)
            {
                ResponseViewModel response= new();
                Log.Error("AccountantSign update error {@Error}", ex.Message); 
                response.DbError();
                responses.Add(response);
            }
            return responses;
        }

        /// <summary>
        /// 將草稿的簽印組狀態變更為待審
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>        
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel Pending(int accountantSignGroupId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSign pending input {@Input}", accountantSignGroupId);
                response = accountantSignService.Pending(accountantSignGroupId);
                Log.Information("AccountantSign pending output {@Output}", response);                
            }
            catch (Exception ex)
            {                
                Log.Error("AccountantSign pending error {@Error}", ex.Message);                 
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 將草稿的簽印組狀態變更為作廢
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel Invalid(int accountantSignGroupId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSign invalid input {@Input}", accountantSignGroupId);
                response = accountantSignService.Invalid(accountantSignGroupId);
                Log.Information("AccountantSign invalid output {@Ouput}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSign Invalid error {@Error}", ex.Message);                 
                response.DbError();                  
            }
            return response;
        }

        /// <summary>
        /// 將待審的簽印組狀態變更為草稿
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel CancelReview(int accountantSignGroupId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSign cancelReview input {@Input}", accountantSignGroupId);
                response = accountantSignService.CancelReview(accountantSignGroupId);
                Log.Information("AccountantSign cancelReview output {@Ouput}", response);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSign CancelReview error {@Error}", ex.Message); 
                response.DbError();
            }
            return response;
        }
    }
}
