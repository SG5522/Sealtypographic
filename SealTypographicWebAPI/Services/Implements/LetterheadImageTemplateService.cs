using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLib.Utils;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities;
using DBEntities.Entities.TemplateModels;
using DBEntities.Utils;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.LetterheadImageTemplate;
using SealTypographicWebAPI.Utils;
using System.Data.Common;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師簽印樣板管理
    /// </summary>
    public class LetterheadImageTemplateService : ILetterheadImageTemplateService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;        
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<LetterheadImageTemplateService> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext">EF Core SealTypographic DbContext</param>        
        /// <param name="mapper">AutoMapper</param>
        /// <param name="imageService">取得圖像資料</param>
        /// <param name="logger">紀錄使用</param>        
        public LetterheadImageTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService, ILogger<LetterheadImageTemplateService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.imageService = imageService;
            this.logger = logger;
        }

        /// <summary>
        /// 信頭樣本詳細
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LetterheadImageTemplateDetailViewModel> GetDetail(int id, int userId = 1)
        {
            logger.LogInformation("GetDetail input {@Input} userId: {@userId} ", id, userId);

            LetterheadImageTemplateDetailViewModel? letterheadImageTemplateDetailViewModel = null;

            try
            {                
                letterheadImageTemplateDetailViewModel = await dbContext.Templates
                                                        .Include(x => x.TemplateLocations)
                                                        .Where(x => x.Id == id)
                                                        .ProjectTo<LetterheadImageTemplateDetailViewModel>(configurationProvider)
                                                        .FirstOrDefaultAsync();

                if (letterheadImageTemplateDetailViewModel != null)
                {
                    letterheadImageTemplateDetailViewModel.Success();
                }
                else
                {
                    letterheadImageTemplateDetailViewModel = new();
                    letterheadImageTemplateDetailViewModel.DbNoData();
                }
                logger.LogInformation("GetDetail output {@Output}", letterheadImageTemplateDetailViewModel);                
            }
            catch (Exception ex) 
            {
                logger.LogError("GetDetail error {@Error}", ex.Message);
                letterheadImageTemplateDetailViewModel = new();
                letterheadImageTemplateDetailViewModel.DbError();
            }            

            return letterheadImageTemplateDetailViewModel;
        }

        /// <summary>
        /// 會計師簽印樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LetterheadImageTemplateImageView> GetImage(int id, int userId = 1)
        {
            logger.LogInformation("GetImage input {@Input} userId: {@userId} ", id, userId);

            LetterheadImageTemplateImageView imageView = new();

            try
            {
                string? imagePath = await dbContext.Templates
                        .Where(x => x.Id == id)
                        .Select(x => x.ImageViewFullPath)
                        .FirstOrDefaultAsync();

                if (imagePath != null)
                {                    
                    imageView.ImageBase64 = imageService.GetPathToBase64(imagePath);
                    imageView.Success();
                }
                logger.LogInformation("GetImage output path {@Output}", imageView);
            }
            catch (Exception ex)
            {
                logger.LogError("GetImage error {@Error}", ex.Message);                
                imageView.DbError();
            }

            return imageView;
        }

        /// <summary>
        /// 信頭樣板分頁顯示
        /// </summary>
        /// <param name="letterheadImageTemplateSearch">信頭樣板分頁搜尋</param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<LetterheadImageTemplatePaginate> GetPaginate(LetterheadImageTemplateSearch letterheadImageTemplateSearch, int userId = 1, int companyId = 1)
        {
            logger.LogInformation("GetPaginate input {@Input} userId: {@userId} ", letterheadImageTemplateSearch, userId);

            LetterheadImageTemplatePaginate letterheadImageTemplatePaginate = new ();
            
            try
            {
                IQueryable<Template> templateQuery = dbContext.Templates
                                                .Where
                                                (
                                                    template => template.Company.Id == companyId
                                                    && template.DeleteStatus == DeleteStatus.No
                                                    && template.TemplateLocations.Any(x => x.SealType == SealType.Letterhead)
                                                );

                if (!string.IsNullOrEmpty(letterheadImageTemplateSearch.KeyWord))
                {
                    templateQuery = templateQuery.Where
                                    (
                                        letterheadImageTemplate => letterheadImageTemplate.Name.Contains(letterheadImageTemplateSearch.KeyWord)
                                    );
                }
                templateQuery = templateQuery.OrderBy(temporarySealGroup => temporarySealGroup.Id);

                if (templateQuery.Any())
                {
                    letterheadImageTemplatePaginate.ViewModels = await PageUtil.SetPaginateViewModelAsync<Template, LetterheadImageTemplateViewModel>
                                                                (
                                                                    templateQuery,
                                                                    configurationProvider,
                                                                    letterheadImageTemplateSearch.PageNumber,
                                                                    letterheadImageTemplateSearch.PageSize
                                                                );

                    PageUtil.SetPaginate(letterheadImageTemplatePaginate, letterheadImageTemplateSearch.PageNumber, letterheadImageTemplateSearch.PageSize, templateQuery.Count());
                    letterheadImageTemplatePaginate.Success();
                }
                else
                {
                    letterheadImageTemplatePaginate.DbNoData();
                }
                logger.LogInformation("GetPaginate output {@Output}", mapper.Map<LetterheadImageTemplatePaginate>(letterheadImageTemplatePaginate));                
            }
            catch (Exception ex)
            {
                logger.LogError("GetPaginate error {@Error}", ex.Message);                
                letterheadImageTemplatePaginate.DbError();
            }            
            
            return letterheadImageTemplatePaginate;
        }

        /// <summary>
        /// 信頭簽印樣板
        /// </summary>
        /// <param name="letterheadImageTemplateForm">信頭樣板</param>
        /// <param name="userId">帳號驗證取得ID</param>
        /// <param name="companyId">公司ID 預設為1</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(LetterheadImageTemplateForm letterheadImageTemplateForm, int userId = 1, int companyId = 1)
        {
            ResponseViewModel response = new();

            try
            {
                logger.LogInformation("New input {@Input} userId: {@userId} ", letterheadImageTemplateForm, userId);
                //尋找公司並與會計師簽印關聯
                Company? companyQuery = dbContext.Companys
                                        .Include(x => x.Templates)
                                        .FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
                {
                    Template template = mapper.Map<Template>(letterheadImageTemplateForm);
                    List<TemplateLocation> templateLocations = new();

                    //儲存圖片(原圖)
                    ImageSaveInfo imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, SealType.Letterhead);
                    imageBase64Info.ImageBase64 = letterheadImageTemplateForm.ImageBase64;
                    template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                    //儲存縮圖
                    imageBase64Info.ImageBase64 = letterheadImageTemplateForm.ImageBase64Thumbnail;
                    template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                    NewTemplateLoction(letterheadImageTemplateForm.LetterheadTemplateLocationForm, templateLocations);
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

                logger.LogInformation("New output {@Output}", response);
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
        /// 更新信頭樣板
        /// </summary>        
        /// <param name="letterheadImageTemplateUpdateForm">信頭樣板</param>
        /// <param name="userId">帳號驗證取得ID</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Update(LetterheadImageTemplateUpdateForm letterheadImageTemplateUpdateForm, int userId = 1)
        {
            ResponseViewModel response = new ();

            try
            {
                logger.LogInformation("Update input {@Input} userId: {@userId} ", letterheadImageTemplateUpdateForm, userId);

                Template? template = dbContext.Templates.Include(x => x.TemplateLocations)
                                    .Include(x => x.Company)
                                    .FirstOrDefault(x => x.Id == letterheadImageTemplateUpdateForm.Id);

                if (template != null)
                {
                    //刪除原圖與縮圖
                    FileUtil.DeleteFile(template.ImageViewFullPath);
                    FileUtil.DeleteFile(template.ThumbnailFullPath);
                    //儲存圖片(原圖)                
                    ImageSaveInfo imageBase64Info = imageService.SetImageBase64InfoWithTemplate(template.Company.Code, SealType.Letterhead);
                    imageBase64Info.ImageBase64 = letterheadImageTemplateUpdateForm.ImageBase64;
                    template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                    //儲存縮圖
                    imageBase64Info.ImageBase64 = letterheadImageTemplateUpdateForm.ImageBase64Thumbnail;
                    template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                    mapper.Map(letterheadImageTemplateUpdateForm, template);
                    InputUtil.Set(template, userId, false);

                    TemplateLocation? templateLocation = template.TemplateLocations.FirstOrDefault(x => x.Id == letterheadImageTemplateUpdateForm.LocationUpdateForm.Id);
                    if (templateLocation != null)
                    {
                        mapper.Map(letterheadImageTemplateUpdateForm.LocationUpdateForm, templateLocation);
                    }

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
                logger.LogError("Update Error while updating database {@error}", ex.InnerException?.Message);
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
        /// 刪除信頭樣板
        /// </summary>
        /// <param name="id">會計師簽印樣板Id</param>
        /// <param name="userId">帳號驗證取得ID</param>
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
                    response.DeleteLetterImageTemplateNoData();
                }
                logger.LogInformation("Delete output {@Output} ", response);
            }
            catch (DbException ex)
            {
                logger.LogError("Delete Error while updating database {@error}", ex.InnerException?.Message);
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
        /// <param name="letterheadImageTemplateLocationForm"></param>
        /// <param name="templateLocations"></param>
        private void NewTemplateLoction(LetterheadImageTemplateLocationForm letterheadImageTemplateLocationForm, List<TemplateLocation> templateLocations)
        {
            TemplateLocation templateLocation = mapper.Map<TemplateLocation>(letterheadImageTemplateLocationForm);
            templateLocation.SealType = SealType.Letterhead;            
            templateLocation.SubSealType = SubSealType.Letterhead;
            templateLocations.Add(templateLocation);            
        }
    }
}
