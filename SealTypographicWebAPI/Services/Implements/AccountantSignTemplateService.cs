using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLib.Utils;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities;
using DBEntities.Entities.TemplateModels;
using DBEntities.Utils;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Utils;
using Serilog;
using System.Drawing.Printing;

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
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext">EF Core SealTypographic DbContext</param>        
        /// <param name="mapper">AutoMapper</param>
        /// <param name="imageService">取得圖像資料</param>        
        public AccountantSignTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageService = imageService;
            configurationProvider = mapper.ConfigurationProvider;
        }

        /// <summary>
        /// 會計師簽印樣本詳細
        /// </summary>
        /// <returns></returns>
        public AccountantSignTemplateDetailViewModel GetDetail(int Id)
        {
            AccountantSignTemplateDetailViewModel? accountantSignTemplateDetailViewModel = dbContext.Templates.Include(x => x.TemplateLocations)
                                                                                            .Include(x => x.TemplateLocations)
                                                                                            .Where(x => x.Id == Id)
                                                                                            .ProjectTo<AccountantSignTemplateDetailViewModel>(configurationProvider)
                                                                                            .FirstOrDefault();

            if (accountantSignTemplateDetailViewModel != null) 
            {
                accountantSignTemplateDetailViewModel.Success();
            }
            else
            {
                accountantSignTemplateDetailViewModel = new();
                accountantSignTemplateDetailViewModel.DbNoData();
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

            string? imagePath = dbContext.Templates.Where(x => x.Id == id)
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

            IQueryable<Template> templateQuery = dbContext.Templates.Where
                                                (
                                                    template => template.Company.Id == companyId
                                                    && template.DeleteStatus == DeleteStatus.No
                                                    && template.TemplateLocations.Any(x => x.SealType == SealType.Accountant)
                                                );

            if (!string.IsNullOrEmpty(accountantSignTemplateSearch.KeyWord))
            {
                templateQuery = templateQuery.Where
                                (
                                    accountantSignTemplate => accountantSignTemplate.Name.Contains(accountantSignTemplateSearch.KeyWord)                
                                );
            }
            templateQuery = templateQuery.OrderBy(temporarySealGroup => temporarySealGroup.Id);

            if (templateQuery.Any())
            {
                accountantSignTemplatePaginate.ViewModels = templateQuery
                                                            .Skip((accountantSignTemplateSearch.PageNumber - 1) * accountantSignTemplateSearch.PageSize)
                                                            .Take(accountantSignTemplateSearch.PageSize)
                                                            .ProjectTo<AccountantSignTemplateViewModel>(configurationProvider)
                                                            .ToList();

                PageUtil.SetPaginate(accountantSignTemplatePaginate, accountantSignTemplateSearch.PageNumber, accountantSignTemplateSearch.PageSize, templateQuery.Count());
                accountantSignTemplatePaginate.Success();
            }
            SavePaginateLog(accountantSignTemplatePaginate);
            return accountantSignTemplatePaginate;
        }

        /// <summary>
        /// 新增會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateForm">會計師簽印樣板</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(AccountantSignTemplateForm accountantSignTemplateForm, int userId = 1)
        {
            ResponseViewModel response = new();                        
            int companyId = 1; //公司ID

            //尋找公司並與會計師簽印關聯
            Company? companyQuery = dbContext.Companys
                                    .Include(x => x.Templates)
                                    .FirstOrDefault(x => x.Id == companyId);

            if (companyQuery != null) 
            {
                Template template = mapper.Map<Template>(accountantSignTemplateForm);
                List<TemplateLocation> templateLocations = new();
                //儲存圖片(原圖)
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, SealType.Accountant);
                imageBase64Info.ImageBase64 = accountantSignTemplateForm.ImageBase64;
                template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                //儲存縮圖
                imageBase64Info.ImageBase64 = accountantSignTemplateForm.ImageBase64Thumbnail;
                template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);
                
                NewTemplateLoction(accountantSignTemplateForm.AccountantSignTemplateLocationForms, templateLocations);                
                InputUtil.Set(template, true, userId);
                template.TemplateLocations = templateLocations;
                template.Company = companyQuery;
                dbContext.Templates.Add(template);                

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
            Template? template = dbContext.Templates.Include(x => x.TemplateLocations)
                                .Include(x => x.Company)
                                .FirstOrDefault(x => x.Id == accountantSignTemplateUpdateForm.Id);

            if (template != null)
            {
                //刪除原圖與縮圖
                FileUtil.DeleteFile(template.ImageViewFullPath);
                FileUtil.DeleteFile(template.ThumbnailFullPath);
                //儲存圖片(原圖)                
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithTemplate(template.Company.Code, SealType.Accountant);
                imageBase64Info.ImageBase64 = accountantSignTemplateUpdateForm.ImageBase64;
                template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                //儲存縮圖
                imageBase64Info.ImageBase64 = accountantSignTemplateUpdateForm.ImageBase64Thumbnail;
                template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                mapper.Map(accountantSignTemplateUpdateForm, template);                
                InputUtil.Set(template, false, userid);

                //刪除樣本座標
                foreach (int deleteLocationId in accountantSignTemplateUpdateForm.DeleteLocationIds)
                {
                    TemplateLocation? templateLocation = template.TemplateLocations.FirstOrDefault(x => x.Id == deleteLocationId);
                    if (templateLocation != null)
                    {
                        template.TemplateLocations.Remove(templateLocation);                        
                    }
                }
                //修改樣本座標
                foreach (AccountantSignTemplateLocationUpdateForm locationUpdateForm in accountantSignTemplateUpdateForm.LocationUpdateForms)
                {
                    TemplateLocation? templateLocation = template.TemplateLocations.FirstOrDefault(x => x.Id == locationUpdateForm.Id);
                    if(templateLocation != null) 
                    {
                        mapper.Map(locationUpdateForm, templateLocation);
                        templateLocation.SealType = SealType.Accountant;
                        //之後要調整為不用轉型
                        templateLocation.SubSealType = SealMappingConfigUtil.GetSubSealTypeWithAccountant(locationUpdateForm.AccountantSignType);
                    }
                }
                //新增樣本座標
                NewTemplateLoction(accountantSignTemplateUpdateForm.LocationForms, template.TemplateLocations);

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
            int userId = 1;

            Template? templateQuery = dbContext.Templates.Find(Id);

            if(templateQuery != null) 
            {
                templateQuery.DeleteStatus = DeleteStatus.Yes;                
                InputUtil.Set(templateQuery, false, userId);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteAccountantSignTemplateNoData();
            }
            return response;
        }

        /// <summary>
        /// 讀取分頁資料
        /// </summary>        
        /// <param name="templateQuery">樣板</param>
        /// <param name="pageNumber">頁次</param>
        /// <param name="pageSize">頁面大小</param>        
        private List<AccountantSignTemplateViewModel> LoadPaginatedData(IQueryable<Template> templateQuery, int pageNumber, int pageSize)
        {
            return
                templateQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(accountantSignTemplate => new AccountantSignTemplateViewModel()
                {
                    Id = accountantSignTemplate.Id,
                    Name = accountantSignTemplate.Name,
                    ImageFullPath = accountantSignTemplate.ThumbnailFullPath,
                    ThumbnailBase64 = imageService.GetPathToBase64(accountantSignTemplate.ThumbnailFullPath)
                })
                .ToList();
        }

        /// <summary>
        /// 紀錄分頁Log
        /// </summary>
        /// <param name="accountantSignTemplatePaginate"></param>
        private void SavePaginateLog(AccountantSignTemplatePaginate accountantSignTemplatePaginate)
        {
            AccountantSignTemplatePaginateLog accountantSignTemplatePaginateLog = mapper.Map<AccountantSignTemplatePaginateLog>(accountantSignTemplatePaginate);
            accountantSignTemplatePaginateLog.LogModels = mapper.Map<List<AccountantSignTemplateLogModel>>(accountantSignTemplatePaginate.ViewModels);
            Log.Information("AccountantSignTemplate paginate output {@Output}", accountantSignTemplatePaginateLog);
        }


        /// <summary>
        /// 新增樣版位置
        /// </summary>
        /// <param name="accountantSignTemplateLocationForms"></param>
        /// <param name="templateLocations"></param>
        private void NewTemplateLoction(IList<AccountantSignTemplateLocationForm> accountantSignTemplateLocationForms, IList<TemplateLocation> templateLocations)
        {
            foreach (AccountantSignTemplateLocationForm accountantSignTemplateLocationForm in accountantSignTemplateLocationForms)
            {
                TemplateLocation templateLocation = mapper.Map<TemplateLocation>(accountantSignTemplateLocationForm);
                templateLocation.SealType = SealType.Accountant;
                //之後要調整為不用轉型
                templateLocation.SubSealType = SealMappingConfigUtil.GetSubSealTypeWithAccountant((AccountantSignType)accountantSignTemplateLocationForm.AccountantSignType);
                templateLocations.Add(templateLocation);
            }
        }
    }
}
