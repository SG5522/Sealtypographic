using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師資料管理
    /// </summary>
    public class AccountantService : IAccountantService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 注入DB與Mapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public AccountantService(SealTypographicDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得會計師資料
        /// </summary>
        /// <param name="accountantId"></param>
        /// <returns></returns>
        public AccountantDetailResponse GetDetail(int accountantId)
        {
            AccountantDetailResponse accountantResponse = new();            

            Accountant? accountantQuery = dbContext.Accountants.Include(accountant => accountant.AccountantGroup)
                                                               .FirstOrDefault(accountant => accountant.Id == accountantId);

            if (accountantQuery != null)
            {
                accountantResponse.AccountantDetailViewModel = mapper.Map<AccountantDetailViewModel>(accountantQuery);
                accountantResponse.Success();
            }
            else
            {
                accountantResponse.AccountantNoData();
            }
            return accountantResponse;
        }

        /// <summary>
        /// 依搜尋條件獲得會計資料列表
        /// </summary>
        /// <param name="accountantSearch">會計師分頁搜尋</param> 
        /// <returns></returns>
        public AccountantPaginateViewModel GetPaginate(AccountantSearch accountantSearch)
        {
            AccountantPaginateViewModel accountantPaginatesViewModels = new();            

            IQueryable<Accountant> accountantQuery = dbContext.Accountants.Where(accountant => accountant.DeleteStatus == DeleteStatus.NO)
                                                    .Include(accountant => accountant.AccountantSignJournals);
            if (!string.IsNullOrWhiteSpace(accountantSearch.NumberOrNameOrGroupsName))
            {
                accountantQuery = accountantQuery.Where
                                (
                                    accountant =>
                                    accountant.Code.ToLower().Contains(accountantSearch.NumberOrNameOrGroupsName.ToLower())                                                         
                                    || accountant.Name.Contains(accountantSearch.NumberOrNameOrGroupsName)
                                    || accountant.AccountantGroup.Name.Contains(accountantSearch.NumberOrNameOrGroupsName)
                                );                                                   
            }

            accountantQuery = accountantQuery.OrderBy(accountant => accountant.Id);
            if (accountantQuery.Any())
            {
                //取得該頁            
                List<Accountant> thisPageAccountants = accountantQuery
                                          .Include(accountantGroup => accountantGroup.AccountantGroup)
                                          .Skip((accountantSearch.PageNumber - 1) * accountantSearch.PageSize)
                                          .Take(accountantSearch.PageSize)
                                          .ToList();
                foreach (Accountant accountant in thisPageAccountants)
                {
                    AccountantViewModelWithCreateDate accountantPaginatesViewModel = mapper.Map<AccountantViewModelWithCreateDate>(accountant);

                    if (accountant.AccountantSignJournals.Count > 0)
                    {
                        accountantPaginatesViewModel.GroupCreateDate = dbContext.SealReviewJournals
                                                                        .Where(x => x.AccountantSignJournal.AccountantId == accountant.Id)
                                                                        .Max(x => x.CreateDate);
                    }
                    accountantPaginatesViewModels.ViewModels.Add(accountantPaginatesViewModel);                    
                }                
                accountantPaginatesViewModels.PageNumber= accountantSearch.PageNumber;
                accountantPaginatesViewModels.PageSize = accountantSearch.PageSize;
                //計算總頁數
                accountantPaginatesViewModels.TotalPage = TotalPageUtil.GetTotalPage(accountantQuery.Count(), accountantSearch.PageSize);               
                accountantPaginatesViewModels.TotalCount = accountantQuery.Count();
                accountantPaginatesViewModels.Success();
            }
            else
            {
                accountantPaginatesViewModels.AccountantNoData();                
            }
            return accountantPaginatesViewModels;
        }

        /// <summary>
        /// 新增會計基本資料
        /// </summary>
        /// <param name="accountantForm">基本資料</param>
        /// <returns></returns>
        public AccountantCreateResponse Create(AccountantForm accountantForm)
        {
            AccountantCreateResponse accountantCreateResponse = new();
            int userid = 0;//帳號驗證取得ID
            Accountant? accountantQuery = dbContext.Accountants
                                    .FirstOrDefault(accountant => accountant.Code == accountantForm.AccountantNumber);                               

            if (accountantQuery == null)
            {
                Accountant dbAccountant = mapper.Map<Accountant>(accountantForm);
                BaseInputAccountant(dbAccountant, true, userid);
                dbContext.Accountants.Add(dbAccountant);
                dbContext.SaveChanges();

                //回傳剛建立的客戶基本資料 使建立客戶印鑑找到該ID
                Accountant? accountant = dbContext.Accountants.Find(dbAccountant.Id);
                                    
                if (accountant != null)
                {
                    accountantCreateResponse.AccountantId = accountant.Id;
                    accountantCreateResponse.Success();
                }
                else
                {
                    accountantCreateResponse.CreateAccountantFailed();
                }                                
            }
            else
            {
                accountantCreateResponse.AccountantNumberRepeat();                
            }
            return accountantCreateResponse;
        }

        /// <summary>
        /// 更新會計師基本資料
        /// </summary>
        /// <param name="accountantFormUpdate">會計師基本資料 accountantBaseData.id 為搜尋條件</param>        
        public ResponseViewModel Update(AccountantFormUpdate accountantFormUpdate)
        {
            ResponseViewModel response = new();
            int userid = 0;//帳號驗證取得ID
            Accountant? accountantQuery = dbContext.Accountants.Find(accountantFormUpdate.Id);

            if (accountantQuery != null)
            {
                mapper.Map(accountantFormUpdate, accountantQuery);
                BaseInputAccountant(accountantQuery, true, userid);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.UpdateAccountantNoData();
            }
            return response;
        }

        /// <summary>
        /// 變更此客戶狀態為刪除(隱藏)。
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        public ResponseViewModel Delete(int accountantId)
        {
            ResponseViewModel response = new();
            int userid = 0;//帳號驗證取得ID
            Accountant? accountantQuery = dbContext.Accountants.Find(accountantId);

            if (accountantQuery != null)
            {
                accountantQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputAccountant(accountantQuery, true, userid);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteAccountantNoData();
            }
            return response;
        }
        /// <summary>
        /// 信頭資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="accountant">DB上的客戶資料</param>
        /// <param name="isCreate">確認是否新增的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputAccountant(Accountant accountant, bool isCreate, int userid)
        {
            if (isCreate)
            {
                accountant.CreateUserId = userid;
                accountant.CreateDate = DateTime.Now;
                accountant.DeleteStatus = DeleteStatus.NO;
            }
            else
            {
                accountant.UpdateUserId = userid;
                accountant.UpdateDate = DateTime.Now;
            }
        }
    }
}