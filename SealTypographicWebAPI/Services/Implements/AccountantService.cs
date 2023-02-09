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

        ///<inheritdoc />
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

        ///<inheritdoc />
        public AccountantPaginateViewModel GetPaginate(AccountantSearch accountantSearch)
        {
            AccountantPaginateViewModel accountantPaginatesViewModels = new();

            IQueryable<Accountant> accountantQuery = dbContext.Accountants.Include(accountant => accountant.AccountantSignCreateDateJournals)
                                                    .Where(accountant => accountant.DeleteStatus == DeleteStatus.No);     
            
            if (!string.IsNullOrWhiteSpace(accountantSearch.KeyWord))
            {
                accountantQuery = accountantQuery.Where
                                (
                                    accountant =>
                                    accountant.Code.ToLower().Contains(accountantSearch.KeyWord.ToLower())                                                         
                                    || accountant.Name.Contains(accountantSearch.KeyWord)
                                    || accountant.AccountantGroup.Name.Contains(accountantSearch.KeyWord)
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

                    if(accountant.AccountantSignCreateDateJournals.Count > 0)
                    {
                        accountantPaginatesViewModel.GroupCreateDate = accountant.AccountantSignCreateDateJournals
                                                                    .Where(x => x.DeleteStatus == DeleteStatus.Yes)
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

        ///<inheritdoc />
        public AccountantCreateResponse New(AccountantForm accountantForm)
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

        ///<inheritdoc />      
        public ResponseViewModel Update(AccountantUpdateForm accountantFormUpdate)
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

        ///<inheritdoc />     
        public ResponseViewModel Delete(int accountantId)
        {
            ResponseViewModel response = new();
            int userid = 0;//帳號驗證取得ID
            Accountant? accountantQuery = dbContext.Accountants.Find(accountantId);

            if (accountantQuery != null)
            {
                accountantQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputAccountant(accountantQuery, false, userid);
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
                accountant.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                accountant.UpdateUserId = userid;
                accountant.UpdateDate = DateTime.Now;
            }
        }
    }
}