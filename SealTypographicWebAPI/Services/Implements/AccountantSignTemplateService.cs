using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using CommonLib.Utils;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities;
using DBEntities.Entities.TemplateModels;
using DBEntities.Utils;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.LetterheadImageTemplate;
using SealTypographicWebAPI.Utils;
using Serilog;
using System.Data.Common;

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
        private readonly ILogger<AccountantSignTemplateService> logger;


        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext">EF Core SealTypographic DbContext</param>        
        /// <param name="mapper">AutoMapper</param>
        /// <param name="imageService">取得圖像資料</param>
        /// <param name="logger"></param>        
        public AccountantSignTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService, ILogger<AccountantSignTemplateService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageService = imageService;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        /// <summary>
        /// 會計師簽印樣本詳細
        /// </summary>
        /// <returns></returns>
        public async Task<AccountantSignTemplateDetailViewModel> GetDetail(int id, int userId = 1)
        {
            logger.LogInformation("GetDetail input {@Input} userId: {@userId} ", id, userId);

            AccountantSignTemplateDetailViewModel? accountantSignTemplateDetailViewModel;

            try
            {
                accountantSignTemplateDetailViewModel = await dbContext.Templates
                                                        .Include(x => x.TemplateLocations)
                                                        .Include(x => x.TemplateLocations)
                                                        .Where(x => x.Id == id)
                                                        .ProjectTo<AccountantSignTemplateDetailViewModel>(configurationProvider)
                                                        .FirstOrDefaultAsync();

                if (accountantSignTemplateDetailViewModel != null)
                {
                    accountantSignTemplateDetailViewModel.Success();
                }
                else
                {
                    accountantSignTemplateDetailViewModel = new();
                    accountantSignTemplateDetailViewModel.DbNoData();
                }
                logger.LogInformation("GetDetail output {@Output}", accountantSignTemplateDetailViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError("GetDetail error {@Error}", ex.Message);
                accountantSignTemplateDetailViewModel = new();
                accountantSignTemplateDetailViewModel.Error();
            }            

            return accountantSignTemplateDetailViewModel;
        }

        /// <summary>
        /// 會計師簽印樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AccountantSignTemplateImageView> GetImage(int id, int userId = 1)
        {
            logger.LogInformation("GetImage input {@Input} userId: {@userId} ", id, userId);

            AccountantSignTemplateImageView viewImage = new();

            try
            {
                string? imagePath = await dbContext.Templates
                                    .Where(x => x.Id == id)
                                    .Select(x => x.ImageViewFullPath)
                                    .FirstOrDefaultAsync();

                if (imagePath != null)
                {
                    viewImage.ImageBase64 = imageService.GetPathToBase64(imagePath);
                    viewImage.Success();
                }
                logger.LogInformation("GetImage output {@Output}", viewImage);
            }
            catch (Exception ex)
            {
                logger.LogError("GetImage error {@Error}", ex.Message);
                viewImage.Error();
            }            

            return viewImage;
        }

        /// <summary>
        /// 會計師簽印樣板分頁顯示
        /// </summary>
        /// <param name="accountantSignTemplateSearch">會計師簽印樣板分頁搜尋</param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<AccountantSignTemplatePaginate> GetPaginate(AccountantSignTemplateSearch accountantSignTemplateSearch, int userId = 1, int companyId = 1)
        {
            AccountantSignTemplatePaginate accountantSignTemplatePaginate = new ();

            logger.LogInformation("GetPaginate input {@Input} userId: {@userId} ", accountantSignTemplateSearch, userId);

            try
            {
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
                    accountantSignTemplatePaginate.ViewModels = await PageUtil.SetPaginateViewModelAsync<Template, AccountantSignTemplateViewModel>
                                                                (
                                                                    templateQuery,
                                                                    configurationProvider,
                                                                    accountantSignTemplateSearch.PageNumber,
                                                                    accountantSignTemplateSearch.PageSize
                                                                );

                    PageUtil.SetPaginate(accountantSignTemplatePaginate, accountantSignTemplateSearch.PageNumber, accountantSignTemplateSearch.PageSize, templateQuery.Count());
                    accountantSignTemplatePaginate.Success();
                }
                else 
                {
                    accountantSignTemplatePaginate.DbNoData();
                }
                logger.LogInformation("GetPaginate output {@Output}", mapper.Map<AccountantSignTemplatePaginate>(accountantSignTemplatePaginate));                
            }
            catch (Exception ex)
            {
                logger.LogError("New error {@Error}", ex.Message);
                accountantSignTemplatePaginate.Error();
            }            
            
            return accountantSignTemplatePaginate;
        }

        /// <summary>
        /// 新增會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateForm">會計師簽印樣板</param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(AccountantSignTemplateForm accountantSignTemplateForm, int userId = 1, int companyId = 1)
        {
            logger.LogInformation("New input {@Input} userId: {@userId} ", accountantSignTemplateForm, userId);

            ResponseViewModel response = new();

            try
            {
                //尋找公司並與會計師簽印關聯
                Company? companyQuery = dbContext.Companys
                                        .Include(x => x.Templates)
                                        .FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
                {
                    Template template = mapper.Map<Template>(accountantSignTemplateForm);
                    List<TemplateLocation> templateLocations = new();
                    //儲存圖片(原圖)
                    ImageSaveInfo imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, SealType.Accountant);
                    imageBase64Info.ImageBase64 = accountantSignTemplateForm.ImageBase64;
                    template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                    //儲存縮圖
                    imageBase64Info.ImageBase64 = accountantSignTemplateForm.ImageBase64Thumbnail;
                    template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                    NewTemplateLoction(accountantSignTemplateForm.AccountantSignTemplateLocationForms, templateLocations);
                    InputUtil.Set(template, userId, true);
                    template.TemplateLocations = templateLocations;
                    template.Company = companyQuery;
                    dbContext.Templates.Add(template);

                    await dbContext.SaveChangesAsync();
                    response.Success();
                }
                else
                {
                    response.DbNoData();
                }
                logger.LogInformation("New output {@Output}", accountantSignTemplateForm);
            }
            catch (DbException ex)
            {
                logger.LogError("New error while updating database {@error}", ex.InnerException?.Message);
                response.DbError();
                response.Message = ex.InnerException?.Message;
            }
            catch (Exception ex)
            {
                logger.LogError("New error {@Error}", ex.Message);
                response.Error();
            }
            
            return response;
        }

        /// <summary>
        /// 更新會計師簽印樣板
        /// </summary>        
        /// <param name="accountantSignTemplateUpdateForm">會計師簽印樣板</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Update(AccountantSignTemplateUpdateForm accountantSignTemplateUpdateForm, int userId = 1)
        {
            logger.LogInformation("Update input {@Input} userId: {@userId} ", accountantSignTemplateUpdateForm, userId);

            ResponseViewModel response = new ();

            try
            {
                Template? template = dbContext.Templates.Include(x => x.TemplateLocations)
                                .Include(x => x.Company)
                                .FirstOrDefault(x => x.Id == accountantSignTemplateUpdateForm.Id);

                if (template != null)
                {
                    //刪除原圖與縮圖
                    FileUtil.DeleteFile(template.ImageViewFullPath);
                    FileUtil.DeleteFile(template.ThumbnailFullPath);
                    //儲存圖片(原圖)                
                    ImageSaveInfo imageBase64Info = imageService.SetImageBase64InfoWithTemplate(template.Company.Code, SealType.Accountant);
                    imageBase64Info.ImageBase64 = accountantSignTemplateUpdateForm.ImageBase64;
                    template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                    //儲存縮圖
                    imageBase64Info.ImageBase64 = accountantSignTemplateUpdateForm.ImageBase64Thumbnail;
                    template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                    mapper.Map(accountantSignTemplateUpdateForm, template);
                    InputUtil.Set(template, userId, false);

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
                        if (templateLocation != null)
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
                else
                {
                    response.DbNoData();
                }
                logger.LogInformation("Update output {@Output}", response);
            }
            catch (DbException ex)
            {
                logger.LogError("Update error while updating database {@error}", ex.InnerException?.Message);
                response.DbError();
                response.Message = ex.InnerException?.Message;
            }
            catch (Exception ex)
            {
                logger.LogError("Update error {@Error}", ex.Message);
                response.Error();
            }
            
            return response;
        }

        /// <summary>
        /// 刪除會計師簽印樣板
        /// </summary>
        /// <param name="id">會計師簽印樣板Id</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Delete(int id, int userId = 1)
        {
            logger.LogInformation("Delete input {@Input} userId: {@userId} ", id, userId);

            ResponseViewModel response = new();

            try
            {
                Template? templateQuery = dbContext.Templates.Find(id);

                if (templateQuery != null)
                {
                    templateQuery.DeleteStatus = DeleteStatus.Yes;
                    InputUtil.Set(templateQuery, userId, false);
                    await dbContext.SaveChangesAsync();
                    response.Success();
                }
                else
                {
                    response.DeleteAccountantSignTemplateNoData();
                }
                logger.LogInformation("Delete output {@Output}", response);
            }
            catch (DbException ex)
            {
                logger.LogError("Delete error while updating database {@error}", ex.InnerException?.Message);
                response.DbError();
                response.Message = ex.InnerException?.Message;
            }
            catch (Exception ex)
            {
                logger.LogError("Delete error {@Error}", ex.Message);
                response.Error();
            }
            
            return response;
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
