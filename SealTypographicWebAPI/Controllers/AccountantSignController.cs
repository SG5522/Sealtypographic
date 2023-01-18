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
        public AccountantSignCreateDateViews CreateDates(int accountantID)
        {
            AccountantSignCreateDateViews accountantSignStartDates = new ();
            try
            {
                Log.Information("AccountantSign get CreateDates input {@Input}", accountantID);
                accountantSignStartDates = accountantSignService.GetCreateDates(accountantID);
                Log.Information("AccountantSign get CreateDates output {@Output}", accountantSignStartDates);                        
            }
            catch (Exception ex)
            {             
                Log.Error("AccountantSign get CreateDates error {@Error}", ex);
                accountantSignStartDates.DbError();                
            }
            return accountantSignStartDates;
        }

        /// <summary>
        /// 依建立日期取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignCreateDate">會計師簽印搜尋(依會計師ID與創建群組日期)</param>        
        /// <returns></returns>        
        [HttpGet]
        public AccountantSignViewModels SignViewModels([FromQuery] AccountantSignCreateDate accountantSignCreateDate)
        {
            AccountantSignViewModels accountantSignViewModels = new();
            try
            {
                Log.Information("AccountantSign get signViewModels input {@Input}", accountantSignCreateDate);
                accountantSignViewModels = accountantSignService.GetSignViewModels(accountantSignCreateDate);
                Log.Information("AccountantSign get signViewModels output {@Output}", accountantSignViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get signViewModels error {@Error}", ex);
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
        public ResponseViewModel New(AccountantSignForms accountantSignForms)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantSign new input {@Input}", accountantSignForms);
                response = accountantSignService.New(accountantSignForms);
                Log.Information("AccountantSign new output {@Output}", response);                             
            }
            catch (Exception ex)
            {                
                Log.Error("AccountantSign new error {@Error}", ex);
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
        public List<ResponseViewModel> Update(AccountantSignUpdate accountantSignUpdate)
        {
            List<ResponseViewModel> responses= new();
            try
            {
                Log.Information("AccountantSign update input {@Input}", accountantSignUpdate);
                responses = accountantSignService.Update(accountantSignUpdate);
                Log.Information("AccountantSign update output {@Output}", responses);
            }
            catch (Exception ex)
            {
                ResponseViewModel response= new();
                Log.Error("AccountantSign update error {@Error}", ex);
                response.DbError();
                responses.Add(response);
            }
            return responses;
        }

        /// <summary>
        /// 將草稿的簽印組狀態變更為待審
        /// </summary>
        /// <param name="accountantSignCreateDate">會計師簽印搜尋(依會計師ID與創建群組日期)</param>        
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel Pending(AccountantSignCreateDate accountantSignCreateDate)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSign pending input {@Input}", accountantSignCreateDate);
                response = accountantSignService.PendingSigns(accountantSignCreateDate);
                Log.Information("AccountantSign pending output {@Output}", response);                
            }
            catch (Exception ex)
            {                
                Log.Error("AccountantSign pending error {@Error}", ex);                
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 將草稿的簽印組狀態變更為作廢
        /// </summary>
        /// <param name="accountantSignCreateDate">會計師簽印搜尋(依會計師ID與創建群組日期)</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel Invalid(AccountantSignCreateDate accountantSignCreateDate)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSign Invalid input {@Input}", accountantSignCreateDate);
                response = accountantSignService.InvalidSigns(accountantSignCreateDate);
                Log.Information("AccountantSign Invalid output {@Ouput}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSign Invalid error {@Error}", ex);                
                response.DbError();                  
            }
            return response;
        }
    }
}
