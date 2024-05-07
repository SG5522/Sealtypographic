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
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Utils;
using System.Data.Common;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶印鑑樣板
    /// </summary>
    public class CustomerSealTemplateService : ICustomerSealTemplateService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;       
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<CustomerSealTemplateService> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageService"></param>
        /// <param name="logger"></param>        
        public CustomerSealTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService, ILogger<CustomerSealTemplateService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageService = imageService;
            this.logger = logger;
            configurationProvider = mapper.ConfigurationProvider;
        }

        /// <summary>
        /// 客戶印鑑樣本詳細
        /// </summary>
        /// <returns></returns>
        public async Task<CustomerSealTemplateDetailViewModel> GetDetail(int id, int userId = 1)
        {
            CustomerSealTemplateDetailViewModel? customerSealTemplateDetailViewModel = new();

            logger.LogInformation("GetDetail input {@Input} userId: {@userId} ", id, userId);
            try
            {
                customerSealTemplateDetailViewModel = await dbContext.Templates
                                                    .Include(x => x.TemplateLocations)
                                                    .Where(x => x.Id == id)
                                                    .ProjectTo<CustomerSealTemplateDetailViewModel>(configurationProvider)
                                                    .FirstOrDefaultAsync();

                if (customerSealTemplateDetailViewModel != null)
                {
                    customerSealTemplateDetailViewModel.Success();
                }
                else
                {
                    customerSealTemplateDetailViewModel = new();
                    customerSealTemplateDetailViewModel.DbNoData();
                }
                logger.LogInformation("GetDetail output {@Output}", customerSealTemplateDetailViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError("GetDetail error {@Error}", ex.Message);
                customerSealTemplateDetailViewModel = new();
                customerSealTemplateDetailViewModel.Error();
            }            

            return customerSealTemplateDetailViewModel;
        }

        /// <summary>
        /// 客戶印鑑樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CustomerSealTemplateImageView> GetImage(int id, int userId = 1)
        {
            logger.LogInformation("GetImage input {@Input} userId: {@userId} ", id, userId);

            CustomerSealTemplateImageView viewImage = new();

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
        /// 客戶印鑑樣板分頁顯示
        /// </summary>
        /// <param name="customerSealTemplateSearch"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<CustomerSealTemplatePaginate> GetPaginate(CustomerSealTemplateSearch customerSealTemplateSearch, int userId = 1, int companyId = 1)
        {
            CustomerSealTemplatePaginate customerSealTemplatePaginate = new ();                        

            logger.LogInformation("GetPaginate input {@Input} userId: {@userId} ", customerSealTemplateSearch, userId);
            try
            {
                IQueryable<Template> templateQuery = dbContext.Templates
                                                .Where
                                                (
                                                    templates => templates.Company.Id == companyId
                                                    && templates.DeleteStatus == DeleteStatus.No
                                                    && templates.TemplateLocations.Any(x => x.SealType == SealType.Customer)
                                                );

                if (!string.IsNullOrEmpty(customerSealTemplateSearch.KeyWord))
                {
                    templateQuery = templateQuery.Where
                                    (
                                        customerSealTemplate => customerSealTemplate.Name.Contains(customerSealTemplateSearch.KeyWord)
                                    );
                }
                templateQuery = templateQuery.OrderBy(temporarySealGroup => temporarySealGroup.Id);

                if (templateQuery.Any())
                {
                    customerSealTemplatePaginate.ViewModels = await PageUtil.SetPaginateViewModelAsync<Template, CustomerSealTemplateViewModel>(
                                                                    templateQuery,
                                                                    configurationProvider,
                                                                    customerSealTemplateSearch.PageNumber,
                                                                    customerSealTemplateSearch.PageSize
                                                                );

                    PageUtil.SetPaginate(customerSealTemplatePaginate, customerSealTemplateSearch.PageNumber, customerSealTemplateSearch.PageSize, templateQuery.Count());
                    customerSealTemplatePaginate.Success();
                }
                else
                {
                    customerSealTemplatePaginate.DbNoData();
                }                
                logger.LogInformation("GetPaginate output {@Output}", mapper.Map<CustomerSealTemplatePaginate>(customerSealTemplatePaginate));
            }
            catch (Exception ex)
            {
                logger.LogError("GetPaginate error {@Error}", ex.Message);
                customerSealTemplatePaginate.Error();
            }
            
            return customerSealTemplatePaginate;
        }

        /// <summary>
        /// 新增客戶印鑑樣板
        /// </summary>
        /// <param name="customerSealTemplateForm">客戶樣板</param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(CustomerSealTemplateForm customerSealTemplateForm, int userId = 1, int companyId = 1)
        {
            ResponseViewModel response = new();                        

            logger.LogInformation("New input {@Input} userId: {@userId} ", customerSealTemplateForm, userId);

            try
            {
                //尋找公司並與客戶關聯
                Company? companyQuery = dbContext.Companys
                                        .Include(x => x.Templates)
                                        .FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
                {
                    Template template = mapper.Map<Template>(customerSealTemplateForm);
                    List<TemplateLocation> templateLocations = new();
                    //儲存圖片(原圖)
                    ImageSaveInfo imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, SealType.Customer);
                    imageBase64Info.ImageBase64 = customerSealTemplateForm.ImageBase64;
                    template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                    //儲存縮圖
                    imageBase64Info.ImageBase64 = customerSealTemplateForm.ImageBase64Thumbnail;
                    template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                    NewTemplateLoction(customerSealTemplateForm.CustomerSealTemplateLocationForms, templateLocations);
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
        /// 更新客戶印鑑樣板
        /// </summary>        
        /// <param name="customerSealTemplateUpdateForm">客戶印鑑樣板</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Update(CustomerSealTemplateUpdateForm customerSealTemplateUpdateForm, int userId = 1)
        {
            logger.LogInformation("Update input {@Input} userId: {@userId} ", customerSealTemplateUpdateForm, userId);

            ResponseViewModel response = new ();

            try
            {
                Template? template = dbContext.Templates.Include(x => x.TemplateLocations)
                                .Include(x => x.Company)
                                .FirstOrDefault(x => x.Id == customerSealTemplateUpdateForm.Id);

                if (template != null)
                {
                    //刪除原圖與縮圖
                    FileUtil.DeleteFile(template.ImageViewFullPath);
                    FileUtil.DeleteFile(template.ThumbnailFullPath);
                    //儲存圖片(原圖)                
                    ImageSaveInfo imageBase64Info = imageService.SetImageBase64InfoWithTemplate(template.Company.Code, SealType.Customer);
                    imageBase64Info.ImageBase64 = customerSealTemplateUpdateForm.ImageBase64;
                    template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                    //儲存縮圖
                    imageBase64Info.ImageBase64 = customerSealTemplateUpdateForm.ImageBase64Thumbnail;
                    template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                    mapper.Map(customerSealTemplateUpdateForm, template);
                    InputUtil.Set(template, userId, false);

                    //刪除樣本座標
                    foreach (int deleteLocationId in customerSealTemplateUpdateForm.DeleteLocationIds)
                    {
                        TemplateLocation? templateLocation = template.TemplateLocations.FirstOrDefault(x => x.Id == deleteLocationId);
                        if (templateLocation != null)
                        {
                            template.TemplateLocations.Remove(templateLocation);
                        }
                    }
                    //修改樣本座標
                    foreach (CustomerSealTemplateLocationUpdateForm locationUpdateForm in customerSealTemplateUpdateForm.LocationUpdateForms)
                    {
                        TemplateLocation? templateLocation = template.TemplateLocations.FirstOrDefault(x => x.Id == locationUpdateForm.Id);

                        if (templateLocation != null)
                        {
                            mapper.Map(locationUpdateForm, templateLocation);
                            templateLocation.SealType = SealType.Customer;

                            templateLocation.SubSealType = SealMappingConfigUtil.GetSubSealTypeWithCustomer(locationUpdateForm.CustomerSealType);
                        }
                    }
                    //新增樣本座標
                    NewTemplateLoction(customerSealTemplateUpdateForm.LocationForms, template.TemplateLocations);

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
        /// 刪除客戶印鑑樣板
        /// </summary>
        /// <param name="id">客戶印鑑樣板Id</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Delete(int id, int userId = 1)
        {
            logger.LogInformation("GetDetail input {@Input} userId: {@userId} ", id, userId);

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
                    response.DeleteCustomerSealTemplateNoData();
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
        /// <param name="customerSealTemplateLocationForms"></param>
        /// <param name="templateLocations"></param>
        private void NewTemplateLoction(IList<CustomerSealTemplateLocationForm> customerSealTemplateLocationForms, IList<TemplateLocation> templateLocations)
        {
            foreach (CustomerSealTemplateLocationForm customerSealTemplateLocationForm in customerSealTemplateLocationForms)
            {
                TemplateLocation templateLocation = mapper.Map<TemplateLocation>(customerSealTemplateLocationForm);
                templateLocation.SealType = SealType.Customer;
                //之後要調整為不用轉型
                templateLocation.SubSealType = SealMappingConfigUtil.GetSubSealTypeWithCustomer(customerSealTemplateLocationForm.CustomerSealType);
                templateLocations.Add(templateLocation);
            }
        }
    }
}
