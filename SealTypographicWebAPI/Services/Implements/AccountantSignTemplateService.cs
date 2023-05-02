using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Utils;
using Serilog;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師簽印樣板管理
    /// </summary>
    public class AccountantSignTemplateService : IAccountantSignTemplateService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;        
        private readonly IMapper mapper;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext">EF Core SealTypographic DbContext</param>        
        /// <param name="mapper">AutoMapper</param>
        /// <param name="ImageService">取得圖像資料</param>        
        public AccountantSignTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService ImageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageService = ImageService;
        }

        /// <summary>
        /// 會計師簽印樣本詳細
        /// </summary>
        /// <returns></returns>
        public AccountantSignTemplateDetailViewModel GetDetail(int Id)
        {
            AccountantSignTemplateDetailViewModel accountantSignTemplateDetailViewModel = new ();

            AccountantSignTemplate? accountantSignTemplateQuery = dbContext.AccountantSignTemplates
                                                                      .Include(x => x.AccountantSignTemplateLocations)
                                                                      .FirstOrDefault(x => x.Id == Id);

            if(accountantSignTemplateQuery != null) 
            {
                accountantSignTemplateDetailViewModel = mapper.Map<AccountantSignTemplateDetailViewModel>(accountantSignTemplateQuery);
                accountantSignTemplateDetailViewModel.LocaltionViewModels = mapper.Map<List<AccountantSignTemplateLocationViewModel>>
                                                                                (accountantSignTemplateQuery.AccountantSignTemplateLocations);
                accountantSignTemplateDetailViewModel.Success();
            }

            return accountantSignTemplateDetailViewModel;
        }

        /// <summary>
        /// 會計師簽印樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountantSignTemplateImageView GetImage(int id)
        {
            AccountantSignTemplateImageView viewImage = new();

            string? imagePath = dbContext.AccountantSignTemplates.Where(x => x.Id == id)
                                                                 .Select(x => x.ImageViewFullPath).FirstOrDefault();

            if (imagePath != null)
            {
                viewImage.ImageBase64 = imageService.GetPathToBase64(imagePath);
                viewImage.Success();
            }
            return viewImage;
        }

        /// <summary>
        /// 會計師簽印樣板分頁顯示
        /// </summary>
        /// <param name="accountantSignTemplateSearch">會計師簽印樣板分頁搜尋</param>
        /// <returns></returns>
        public AccountantSignTemplatePaginate GetPaginate(AccountantSignTemplateSearch accountantSignTemplateSearch)
        {
            AccountantSignTemplatePaginate accountantSignTemplatePaginate = new ();            
            int companyId = 1;

            IQueryable<AccountantSignTemplate> accountantSignTemplateQuery = dbContext.AccountantSignTemplates
                                                                    .Where
                                                                    (
                                                                        accountantSignTemplate => accountantSignTemplate.Company.Id == companyId                                                                        
                                                                        && accountantSignTemplate.DeleteStatus == DeleteStatus.No
                                                                    );

            if (!string.IsNullOrEmpty(accountantSignTemplateSearch.KeyWord))
            {
                accountantSignTemplateQuery = accountantSignTemplateQuery
                                            .Where
                                            (
                                                accountantSignTemplate => accountantSignTemplate.Name.Contains(accountantSignTemplateSearch.KeyWord)                
                                            );
            }
            accountantSignTemplateQuery = accountantSignTemplateQuery.OrderBy(temporarySealGroup => temporarySealGroup.Id);

            if (accountantSignTemplateQuery.Any())
            {
                List<AccountantSignTemplateViewModel> thisPageAccountantSignTemplate = accountantSignTemplateQuery                                                                      
                                                                    .Skip((accountantSignTemplateSearch.PageNumber - 1) * accountantSignTemplateSearch.PageSize)
                                                                    .Take(accountantSignTemplateSearch.PageSize)
                                                                    .Select(accountantSignTemplate => new AccountantSignTemplateViewModel()
                                                                    {
                                                                        Id = accountantSignTemplate.Id,
                                                                        Name = accountantSignTemplate.Name,
                                                                        ImageFullPath = accountantSignTemplate.ThumbnailFullPath,
                                                                        ThumbnailBase64 = imageService.GetPathToBase64(accountantSignTemplate.ThumbnailFullPath)
                                                                    })
                                                                    .ToList();

                accountantSignTemplatePaginate.ViewModels = thisPageAccountantSignTemplate;
                accountantSignTemplatePaginate.PageNumber = accountantSignTemplateSearch.PageNumber;
                accountantSignTemplatePaginate.PageSize = accountantSignTemplateSearch.PageSize;
                //計算總頁數
                accountantSignTemplatePaginate.TotalPage = TotalPageUtil.GetTotalPage(accountantSignTemplateQuery.Count(), accountantSignTemplateSearch.PageSize);
                accountantSignTemplatePaginate.TotalCount = accountantSignTemplateQuery.Count();
                accountantSignTemplatePaginate.Success();
            }
            AccountantSignTemplatePaginateLog accountantSignTemplatePaginateLog = mapper.Map<AccountantSignTemplatePaginateLog>(accountantSignTemplatePaginate);
            accountantSignTemplatePaginateLog.LogModels = mapper.Map<List<AccountantSignTemplateLogModel>>(accountantSignTemplatePaginate.ViewModels);            
            Log.Information("AccountantSignTemplate paginate output {@Output}", accountantSignTemplatePaginate);
            return accountantSignTemplatePaginate;
        }

        /// <summary>
        /// 新增會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateForm">會計師簽印樣板</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(AccountantSignTemplateForm accountantSignTemplateForm)
        {
            ResponseViewModel response = new();            
            int userid = 0; //帳號驗證取得ID
            int companyId = 1; //公司ID

            //尋找公司並與會計師簽印關聯
            Company? companyQuery = dbContext.Companys
                                    .Include(x => x.AccountantSignTemplates)
                                    .Select(x => new Company 
                                    { 
                                        Id = x.Id , 
                                        Code = x.Code ,
                                        AccountantSignTemplates = new List<AccountantSignTemplate>()
                                    })
                                    .FirstOrDefault(x => x.Id == companyId);

            if (companyQuery != null) 
            {
                AccountantSignTemplate accountantSignTemplate = mapper.Map<AccountantSignTemplate>(accountantSignTemplateForm);
                //儲存圖片(原圖)
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, SealType.Accountant);
                imageBase64Info.ImageBase64 = accountantSignTemplateForm.ImageBase64;
                accountantSignTemplate.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                //儲存縮圖
                imageBase64Info.ImageBase64 = accountantSignTemplateForm.ImageBase64Thumbnail;
                accountantSignTemplate.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                List<AccountantSignTemplateLocation> accountantSignTemplateLocations = new();
                foreach (AccountantSignTemplateLocationForm accountantSignTemplateLocationForm in accountantSignTemplateForm.AccountantSignTemplateLocationForms)
                {                   
                    accountantSignTemplateLocations.Add(mapper.Map<AccountantSignTemplateLocation>(accountantSignTemplateLocationForm));                    
                }
                BaseInputAccountantSignTemplate(accountantSignTemplate, true, userid);
                accountantSignTemplate.AccountantSignTemplateLocations = accountantSignTemplateLocations;                
                companyQuery.AccountantSignTemplates.Add(accountantSignTemplate);
                dbContext.Entry(companyQuery).State = EntityState.Unchanged;
                dbContext.AccountantSignTemplates.Add(accountantSignTemplate);                
                await dbContext.SaveChangesAsync();
                response.Success();
            }
            return response;
        }

        /// <summary>
        /// 更新會計師簽印樣板
        /// </summary>        
        /// <param name="accountantSignTemplateUpdateForm">會計師簽印樣板</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Update(AccountantSignTemplateUpdateForm accountantSignTemplateUpdateForm)
        {
            ResponseViewModel response = new ();
            int userid = 1;
            AccountantSignTemplate? accountantSignTemplateQuery = dbContext.AccountantSignTemplates.Include(x => x.AccountantSignTemplateLocations)
                                                                                                   .Include(x => x.Company)
                                                                                                   .FirstOrDefault(x => x.Id == accountantSignTemplateUpdateForm.Id);

            if (accountantSignTemplateQuery != null)
            {
                //儲存圖片(原圖)                
                imageService.SaveImage(accountantSignTemplateUpdateForm.ImageBase64, accountantSignTemplateQuery.ImageViewFullPath, false);
                //儲存縮圖                
                imageService.SaveImage(accountantSignTemplateUpdateForm.ImageBase64Thumbnail, accountantSignTemplateQuery.ThumbnailFullPath, false);

                mapper.Map(accountantSignTemplateUpdateForm, accountantSignTemplateQuery);
                BaseInputAccountantSignTemplate(accountantSignTemplateQuery, false, userid);

                //刪除樣本座標
                foreach (int deleteLocationId in accountantSignTemplateUpdateForm.DeleteLocationIds)
                {
                    AccountantSignTemplateLocation? accountantSignTemplateLocation = accountantSignTemplateQuery.AccountantSignTemplateLocations
                                                                                                          .FirstOrDefault(x => x.Id == deleteLocationId);
                    if (accountantSignTemplateLocation != null)
                    {
                        dbContext.Remove(accountantSignTemplateLocation);
                    }
                }
                //修改樣本座標
                foreach (AccountantSignTemplateLocationUpdateForm signTemplateLocationUpdateForm in accountantSignTemplateUpdateForm.LocationUpdateForms)
                {
                    AccountantSignTemplateLocation? accountantSignTemplateLocation = accountantSignTemplateQuery.AccountantSignTemplateLocations
                                                                                .FirstOrDefault(x => x.Id == signTemplateLocationUpdateForm.Id);
                    if(accountantSignTemplateLocation != null) 
                    {
                        mapper.Map(signTemplateLocationUpdateForm, accountantSignTemplateLocation);
                    }
                }
                //新增樣本座標
                foreach (AccountantSignTemplateLocationForm locationForm in accountantSignTemplateUpdateForm.LocationForms)
                {
                    accountantSignTemplateQuery.AccountantSignTemplateLocations.Add(mapper.Map<AccountantSignTemplateLocation>(locationForm));
                }
                await dbContext.SaveChangesAsync();
                response.Success();
            }           
            return response;
        }
        /// <summary>
        /// 刪除會計師簽印樣板
        /// </summary>
        /// <param name="Id">會計師簽印樣板Id</param>
        /// <returns></returns>
        public ResponseViewModel Delete (int Id)
        {
            ResponseViewModel response = new();
            int userId = 0;

            AccountantSignTemplate? accountantSignTemplateQuery = dbContext.AccountantSignTemplates.Find(Id);

            if(accountantSignTemplateQuery != null) 
            {
                accountantSignTemplateQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputAccountantSignTemplate(accountantSignTemplateQuery, false, userId);
                response.Success();
            }
            else
            {
                response.DeleteAccountantSignTemplateNoData();
            }

            return response;
        }

        /// <summary>
        /// 資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="accountantSignTemplate">DB上的樣板資料</param>
        /// <param name="isCreate">確認是否新增還是更新的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputAccountantSignTemplate(AccountantSignTemplate accountantSignTemplate, bool isCreate, int userid)
        {
            if (isCreate)
            {
                accountantSignTemplate.CreateUserId = userid;
                accountantSignTemplate.CreateDate = DateTime.Now;
                accountantSignTemplate.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                accountantSignTemplate.UpdateUserId = userid;
                accountantSignTemplate.UpdateDate = DateTime.Now;
            }
        }
    }
}
