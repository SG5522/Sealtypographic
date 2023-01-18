using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Utils;
using System.Linq;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師簽印管理
    /// </summary>
    public class AcoountantSignService : IAccountantSignService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageSharpService;
        private readonly IMapper mapper;
        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageSharpService"></param>
        public AcoountantSignService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageSharpService)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            this.imageSharpService = imageSharpService;

        }

        /// <summary>
        /// 取得會計師簽印建立日期列表
        /// </summary>
        /// <param name="accountantId">會計師Id</param>
        /// <returns></returns>
        public AccountantSignCreateDateViews GetCreateDates(int accountantId)
        {
            AccountantSignCreateDateViews accountantSignStartDates = new()
            {
                GroupCreateDates = dbContext.AccountantSignCreateDateJournals
                                                       .Where
                                                       (
                                                            accountantSignCreateDateJournal => accountantSignCreateDateJournal.Accountant.Id == accountantId
                                                            && accountantSignCreateDateJournal.ReviewStatus <= ReviewStatus.Reject
                                                            && accountantSignCreateDateJournal.DeleteStatus == DeleteStatus.No
                                                       )
                                                       .Select(sealReviewJournal => new AccountantSignCreateDateView()
                                                       {
                                                           AccountantId = accountantId,
                                                           GroupCreateDate = sealReviewJournal.CreateDate,
                                                           ReviewStatus = sealReviewJournal.ReviewStatus
                                                       })                                                       
                                                       .OrderByDescending(accountantSignCreateDateView => accountantSignCreateDateView.GroupCreateDate)                                                       
                                                       .ToList()
            };

            if (accountantSignStartDates.GroupCreateDates.Any())
            {                                                
                accountantSignStartDates.Success();
            }
            else
            {
                accountantSignStartDates.AccountantSignNoData();
            }
            return accountantSignStartDates;
        }

        /// <summary>
        /// 依建立日期取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignCreateDate"></param>
        /// <returns></returns>
        public AccountantSignViewModels GetSignViewModels(AccountantSignCreateDate accountantSignCreateDate)
        {
            AccountantSignViewModels signViewModels = new()
            {
                AccountantId = accountantSignCreateDate.AccountantId,
                GroupCreateDate = accountantSignCreateDate.GroupCreateDate
            };
            AccountantSignCreateDateJournal? accountantSignCreateDateJournalQuery = dbContext.AccountantSignCreateDateJournals
                                                                .Include(accountantSignCreateDateJournal => accountantSignCreateDateJournal.AccountantSignJournals)
                                                                .FirstOrDefault
                                                                (
                                                                    accountantSignCreateDateJournal =>
                                                                    accountantSignCreateDateJournal.Accountant.Id == accountantSignCreateDate.AccountantId
                                                                    && accountantSignCreateDateJournal.CreateDate == accountantSignCreateDate.GroupCreateDate
                                                                    //&& customerSealQuarterJournal.Id = customerSealQuarter.Id
                                                                    && accountantSignCreateDateJournal.DeleteStatus == DeleteStatus.No
                                                                    && accountantSignCreateDateJournal.ReviewStatus <= ReviewStatus.Reject
                                                                );

            if (accountantSignCreateDateJournalQuery != null)
            {
                foreach (AccountantSignJournal accountantSignJournal in accountantSignCreateDateJournalQuery.AccountantSignJournals)
                {
                    AccountantSignViewModel accountantSignViewModel = mapper.Map<AccountantSignViewModel>(accountantSignJournal);
                    accountantSignViewModel.ImageBase64 = imageSharpService.GetPathToBase64(accountantSignJournal.ImagePath, SealType.Accountant); //資料庫取得圖檔路徑轉BASE64                   
                    
                    accountantSignViewModel.SealMappingConfigId = accountantSignJournal.ConfigType;
                    signViewModels.SignViewModels.Add(accountantSignViewModel);
                }
                signViewModels.ReviewStatus = accountantSignCreateDateJournalQuery.ReviewStatus;                
                signViewModels.Success();
            }
            else
            {
                signViewModels.AccountantSignNoData();
            }

            return signViewModels;
        }

        /// <summary>
        /// 新增會計師簽印組
        /// </summary>
        /// <param name="accountantSignForms">會計師簽印組</param>
        /// <returns></returns>
        public ResponseViewModel New(AccountantSignForms accountantSignForms)
        {
            ResponseViewModel response = new();
            List<AccountantSignJournal> accountantSignJournals = new();
            int userId = 0; //從帳號驗證取得Id

            //確認是否有一組草稿或待審的會計師簽印
            Accountant? accountantQuery = dbContext.Accountants.Include(accountant => accountant.AccountantSignCreateDateJournals)
                                .FirstOrDefault(accountant => accountant.Id == accountantSignForms.AccountantId);

            if (accountantQuery != null)
            {
                int count = 1;
                AccountantSignCreateDateJournal accountantSignCreateDateJournal = new();
                ImageBase64Info imageBase64Info = new()
                {
                    Code = GetCode(accountantSignForms.AccountantId),                    
                    SealType = SealType.Accountant
                };

                BaseInputCreateDateJournal(accountantSignCreateDateJournal, true, userId);                
                
                foreach (AccountantSign accountantSign in accountantSignForms.SignForms)
                {
                    AccountantSignJournal accountantSignJournal = new()
                    {
                        ConfigType = accountantSign.SealMappingConfigId
                    };
                                        
                    imageBase64Info.ImageBase64 = accountantSign.ImageBase64;
                    accountantSignJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    
                    BaseInputAccountantSignJournal(accountantSignJournal, true, userId);
                    accountantSignJournals.Add(accountantSignJournal);
                    count++;
                }

                accountantSignCreateDateJournal.AccountantSignJournals = accountantSignJournals;
                accountantQuery.AccountantSignCreateDateJournals.Add(accountantSignCreateDateJournal);
                dbContext.SaveChanges();
                response.Success();
            }                   
            else
            {
                response.AccountantNoData();
            }
            return response;
        }
        /// <summary>
        /// 異動會計師簽印
        /// </summary>
        /// <param name="accountantSignUpdate">需要異動會計師簽印資料</param>
        /// <returns></returns>
        public List<ResponseViewModel> Update(AccountantSignUpdate accountantSignUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();            
            int userId = 0;//之後會從帳號驗證中取得userid
            int count = 1;

            ImageBase64Info imageBase64Info = new()
            {
                Code = GetCode(accountantSignUpdate.AccountantId),
                SealType = SealType.Accountant,                
            };

            List<int> updateAccountantSignIds = accountantSignUpdate.UpdateAccountantSigns.Select(x => x.Id).ToList();

            AccountantSignCreateDateJournal? accountantSignCreateDateJournalQuery = dbContext.AccountantSignCreateDateJournals
                                                                                    .Include(accountantSignCreateDateJournal => accountantSignCreateDateJournal.AccountantSignJournals)
                                                                                    .FirstOrDefault
                                                                                    (
                                                                                        accountantSignCreateDateJournal => accountantSignCreateDateJournal.CreateDate == accountantSignUpdate.GroupCreateDate
                                                                                        && accountantSignCreateDateJournal.Accountant.Id == accountantSignUpdate.AccountantId
                                                                                    );
            if(accountantSignCreateDateJournalQuery != null)
            {
                //刪除印鑑
                foreach (int deleteSignId in accountantSignUpdate.DeleteAccountantSignIds)
                {
                    AccountantSignJournal? deleteSignQuery = dbContext.AccountantSignJournals
                                                                .FirstOrDefault
                                                                (
                                                                    accountantSignJournal => accountantSignJournal.Id == deleteSignId
                                                                    && accountantSignJournal.DeleteStatus == DeleteStatus.No
                                                                );
                    if (deleteSignQuery != null)
                    {
                        deleteSignQuery.DeleteStatus = DeleteStatus.Yes;
                        BaseInputAccountantSignJournal(deleteSignQuery, false, userId);
                    }
                    else
                    {
                        ResponseViewModel response = new();
                        response.DeleteAccountantSignNoData();
                        response.ErrorItem = $"Delete AccountantSignid:{deleteSignId}";
                        responseViewModels.Add(response);
                    }
                }

                //修改印鑑
                foreach (AccountantSignFormUpdate accountantSignFormUpdate in accountantSignUpdate.UpdateAccountantSigns)
                {
                    AccountantSignJournal? updateSignQuery = dbContext.AccountantSignJournals
                                                                .FirstOrDefault
                                                                (
                                                                    accountantSignJournal => accountantSignJournal.Id == accountantSignFormUpdate.Id
                                                                    && accountantSignJournal.DeleteStatus == DeleteStatus.No
                                                                );
                    if (updateSignQuery != null)
                    {
                        AccountantSignJournal accountantSignJournal = new()
                        {
                            ConfigType = updateSignQuery.ConfigType,
                        };

                        //ImageBase64轉圖檔並存到指定資料夾                    
                        imageBase64Info.ImageBase64 = accountantSignFormUpdate.ImageBase64;
                        accountantSignJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);
                        BaseInputAccountantSignJournal(accountantSignJournal, true, userId);

                        accountantSignCreateDateJournalQuery.AccountantSignJournals.Add(accountantSignJournal);

                        //原印鑑刪除(Hide)
                        updateSignQuery.DeleteStatus = DeleteStatus.Yes;
                        BaseInputAccountantSignJournal(updateSignQuery, false, userId);

                        count++;
                    }
                    else
                    {
                        ResponseViewModel response = new();
                        response.UpdateAccountantSignNoData();
                        response.ErrorItem = $"Update AccountantSignid:{accountantSignFormUpdate.Id}";
                        responseViewModels.Add(response);
                    }
                }

                //新增印鑑
                foreach (AccountantSign createAccountantSign in accountantSignUpdate.CreateAccountantSigns)
                {
                    AccountantSignCheck accountantSignCheck = new()
                    {
                        AccountantSignCreateDateJournalId = accountantSignCreateDateJournalQuery.Id,
                        SealMappingConfigId = createAccountantSign.SealMappingConfigId,                        
                    };
                    
                    if (!CheckAccountSignRepeat(accountantSignCheck, accountantSignUpdate.DeleteAccountantSignIds, updateAccountantSignIds))
                    {
                        AccountantSignJournal accountantSignJournal = new()
                        {                            
                            ConfigType = createAccountantSign.SealMappingConfigId
                        };

                        //ImageBase64轉圖檔並存到指定資料夾
                        imageBase64Info.ImageBase64 = createAccountantSign.ImageBase64;
                        accountantSignJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);

                        BaseInputAccountantSignJournal(accountantSignJournal, true, userId);
                        accountantSignCreateDateJournalQuery.AccountantSignJournals.Add(accountantSignJournal);

                        count++;
                    }
                    else
                    {
                        ResponseViewModel response = new();
                        response.CreateAccountantSignRepeat();
                        response.ErrorItem = $"New SealMappingConfigId:{createAccountantSign.SealMappingConfigId}";
                        responseViewModels.Add(response);
                    }
                }

                //沒有任何回傳訊息(錯誤訊息)就更新資料庫
                if (!responseViewModels.Any())
                {
                    ResponseViewModel response = new();
                    accountantSignCreateDateJournalQuery.ReviewStatus = ReviewStatus.Draft;

                    dbContext.SaveChanges();
                    response.Success();
                    responseViewModels.Add(response);
                }
            }
            else
            {
                ResponseViewModel response = new();
                response.AccountantSignNoData();
            }
            
            return responseViewModels;
        }


        /// <summary>
        /// 將草稿的簽印組狀態變更為待審
        /// </summary>
        /// <param name="accountantSignCreateDate">會計師簽印搜尋(依會計師ID與創建群組日期)</param>        
        /// <returns></returns>
        public ResponseViewModel PendingSigns(AccountantSignCreateDate accountantSignCreateDate)
        {            
            ResponseViewModel response = ChangeDraftReviewStatus(accountantSignCreateDate, ReviewStatus.Pending);
            return response;
        }

        /// <summary>
        /// 將草稿的簽印組狀態變更為作廢
        /// </summary>
        /// <param name="accountantSignCreateDate">會計師簽印搜尋(依會計師ID與創建群組日期)</param>        
        public ResponseViewModel InvalidSigns(AccountantSignCreateDate accountantSignCreateDate)
        {
            ResponseViewModel response = ChangeDraftReviewStatus(accountantSignCreateDate, ReviewStatus.Invalid);
            return response;
        }

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="accountantSignJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">建立或更新此檔的user的Id</param>
        private static void BaseInputAccountantSignJournal(AccountantSignJournal accountantSignJournal, bool isCreate , int userId)
        {
            if (isCreate)
            {
                accountantSignJournal.CreateUserId = userId;
                accountantSignJournal.CreateDate = DateTime.Now;
                accountantSignJournal.DeleteStatus = DeleteStatus.No;                                
            }
            else
            {
                accountantSignJournal.UpdateUserId = userId;
                accountantSignJournal.UpdateDate = DateTime.Now;                
            }
        }

        /// <summary>
        /// 會計師簽印建立日期歷程基本資料輸入
        /// </summary>
        /// <param name="accountantSignCreateDateJournal">會計師簽印建立日期歷程</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputCreateDateJournal(AccountantSignCreateDateJournal accountantSignCreateDateJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                accountantSignCreateDateJournal.CreateUserId = userId;
                accountantSignCreateDateJournal.CreateDate = DateTime.Now;
                accountantSignCreateDateJournal.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                accountantSignCreateDateJournal.UpdateUserId = userId;
                accountantSignCreateDateJournal.UpdateDate = DateTime.Now;
            }
            accountantSignCreateDateJournal.StartDate = AvailableDateUtil.NotActivated();
            accountantSignCreateDateJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            accountantSignCreateDateJournal.ReviewStatus = ReviewStatus.Draft;
        }

        /// <summary>
        /// 確認此類別會計師簽印是否重覆建立 true 重複 false 不重複
        /// </summary>
        /// <param name="accountantSignCheck">查詢參數</param>
        /// <param name="DeleteAccountantSignIds">異動中刪除的簽印Id</param>
        /// <param name="updateAccountantSignIds">異動中更新的簽印Id(也會標上刪除)</param>
        /// <returns></returns>
        private bool CheckAccountSignRepeat(AccountantSignCheck accountantSignCheck, List<int> DeleteAccountantSignIds, List<int> updateAccountantSignIds)
        {
            AccountantSignJournal? signQuery = dbContext.AccountantSignJournals
                                            .FirstOrDefault
                                            (
                                                accountantSignJournal => accountantSignJournal.AccountantSignCreateDateJournal.Id == accountantSignCheck.AccountantSignCreateDateJournalId
                                                && accountantSignJournal.ConfigType == accountantSignCheck.SealMappingConfigId
                                                && accountantSignJournal.DeleteStatus == DeleteStatus.No
                                                && !DeleteAccountantSignIds.Contains(accountantSignJournal.Id)
                                                && !updateAccountantSignIds.Contains(accountantSignJournal.Id)
                                            );
            return signQuery != null;
        }

        /// <summary>
        /// 會計師印鑑待審狀態變更。
        /// </summary>
        /// <param name="accountantSignCreateDate"></param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeDraftReviewStatus(AccountantSignCreateDate accountantSignCreateDate, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userid = 0; //從帳號驗證取得Id
            AccountantSignCreateDateJournal? accountantSignCreateDateQuery = dbContext.AccountantSignCreateDateJournals
                                                                        .FirstOrDefault
                                                                        (
                                                                            accountantSignCreateDateJournal => 
                                                                            accountantSignCreateDateJournal.Accountant.Id == accountantSignCreateDate.AccountantId
                                                                            && accountantSignCreateDateJournal.CreateDate == accountantSignCreateDate.GroupCreateDate                                                                            
                                                                            && accountantSignCreateDateJournal.ReviewStatus == ReviewStatus.Draft
                                                                        );
            if(accountantSignCreateDateQuery != null)
            {
                accountantSignCreateDateQuery.ReviewStatus = reviewStatus;
                accountantSignCreateDateQuery.UpdateUserId = userid;
                accountantSignCreateDateQuery.UpdateDate = DateTime.Now;
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.UpdateAccountantSignNoData();                                
            }
            return response;
        }
        private string GetCode(int accountantId)
        {
            string code;
            Accountant? accountant = dbContext.Accountants.Find(accountantId);
            if (accountant != null)
            {
                code = accountant.Code;
            }
            else
            {
                code = string.Empty;
            }
            return code;
        }
    }
}
