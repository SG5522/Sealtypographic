using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Utils;
using Serilog;

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

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageService"></param>        
        public CustomerSealTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageService = imageService;            
        }

        /// <summary>
        /// 客戶印鑑樣本詳細
        /// </summary>
        /// <returns></returns>
        public CustomerSealTemplateDetailViewModel GetDetail(int Id)
        {
            CustomerSealTemplateDetailViewModel customerSealTemplateDetailViewModel = new ();

            CustomerSealTemplate? customerSealTemplateQuery = dbContext.CustomerSealTemplates
                                                                      .Include(x => x.CustomerSealTemplateLocations)
                                                                      .FirstOrDefault(x => x.Id == Id);

            if(customerSealTemplateQuery != null) 
            {
                customerSealTemplateDetailViewModel = mapper.Map<CustomerSealTemplateDetailViewModel>(customerSealTemplateQuery);
                customerSealTemplateDetailViewModel.LocaltionViewModels = mapper.Map<List<CustomerSealTemplateLocationViewModel>>
                                                                            (customerSealTemplateQuery.CustomerSealTemplateLocations);
                customerSealTemplateDetailViewModel.Success();
            }

            return customerSealTemplateDetailViewModel;
        }

        /// <summary>
        /// 客戶印鑑樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerSealTemplateImageView GetImage(int id)
        {
            CustomerSealTemplateImageView viewImage = new();

            string? imagePath = dbContext.CustomerSealTemplates.Where(x => x.Id == id)
                                                               .Select(x => x.ImageViewFullPath).FirstOrDefault();       

            if(imagePath != null)
            {
                viewImage.ImageBase64 = imageService.GetPathToBase64(imagePath);
                viewImage.Success();
            }
            return viewImage;
        }

        /// <summary>
        /// 客戶印鑑樣板分頁顯示
        /// </summary>
        /// <param name="customerSealTemplateSearch"></param>
        /// <returns></returns>
        public CustomerSealTemplatePaginate GetPaginate(CustomerSealTemplateSearch customerSealTemplateSearch)
        {
            CustomerSealTemplatePaginate customerSealTemplatePaginate = new ();            
            int companyId = 1;

            IQueryable<CustomerSealTemplate> customerSealTemplateQuery = dbContext.CustomerSealTemplates
                                                                        .Where
                                                                        (
                                                                            customerSealTemplate => customerSealTemplate.Company.Id == companyId                                                                        
                                                                            && customerSealTemplate.DeleteStatus == DeleteStatus.No
                                                                        );

            if (!string.IsNullOrEmpty(customerSealTemplateSearch.KeyWord))
            {
                customerSealTemplateQuery = customerSealTemplateQuery
                                            .Where
                                            (
                                                customerSealTemplate => customerSealTemplate.Name.Contains(customerSealTemplateSearch.KeyWord)                
                                            );
            }
            customerSealTemplateQuery = customerSealTemplateQuery.OrderBy(temporarySealGroup => temporarySealGroup.Id);

            if (customerSealTemplateQuery.Any())
            {
                customerSealTemplatePaginate.ViewModels = LoadPaginatedData(customerSealTemplateQuery, customerSealTemplateSearch.PageNumber, customerSealTemplateSearch.PageSize);
                customerSealTemplatePaginate.PageNumber = customerSealTemplateSearch.PageNumber;
                customerSealTemplatePaginate.PageSize = customerSealTemplateSearch.PageSize;
                //計算總頁數
                customerSealTemplatePaginate.TotalPage = TotalPageUtil.GetTotalPage(customerSealTemplateQuery.Count(), customerSealTemplateSearch.PageSize);
                customerSealTemplatePaginate.TotalCount = customerSealTemplateQuery.Count();
                customerSealTemplatePaginate.Success();
            }
            SavePaginateLog(customerSealTemplatePaginate);
            return customerSealTemplatePaginate;
        }        

        /// <summary>
        /// 新增客戶印鑑樣板
        /// </summary>
        /// <param name="customerSealTemplateForm">客戶樣板</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(CustomerSealTemplateForm customerSealTemplateForm)
        {
            ResponseViewModel response = new();            
            int userid = 0; //帳號驗證取得ID
            int companyId = 1; //公司ID

            //尋找公司並與客戶關聯
            Company? companyQuery = dbContext.Companys
                                    .Include(x => x.CustomerSealTemplates)
                                    .Select(x => new Company 
                                    { 
                                        Id = x.Id , 
                                        Code = x.Code ,
                                        CustomerSealTemplates = new List<CustomerSealTemplate>()
                                    })
                                    .FirstOrDefault(x => x.Id == companyId);

            if (companyQuery != null) 
            {                
                CustomerSealTemplate customerSealTemplate = mapper.Map<CustomerSealTemplate>(customerSealTemplateForm);
                //儲存圖片(原圖)
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, SealType.Customer);

                imageBase64Info.ImageBase64 = customerSealTemplateForm.ImageBase64;
                customerSealTemplate.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                //儲存縮圖
                imageBase64Info.ImageBase64 = customerSealTemplateForm.ImageBase64Thumbnail;
                customerSealTemplate.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                List<CustomerSealTemplateLocation> customerSealTemplateLocations = new();
                foreach (CustomerSealTemplateLocationForm customerSealTemplateLocationForm in customerSealTemplateForm.CustomerSealTemplateLocationForms)
                {                   
                    customerSealTemplateLocations.Add(mapper.Map<CustomerSealTemplateLocation>(customerSealTemplateLocationForm));                    
                }
                BaseInputCustomerSealTemplate(customerSealTemplate, true, userid);
                customerSealTemplate.CustomerSealTemplateLocations = customerSealTemplateLocations;                
                companyQuery.CustomerSealTemplates.Add(customerSealTemplate);

                dbContext.Entry(companyQuery).State = EntityState.Unchanged;
                dbContext.CustomerSealTemplates.Add(customerSealTemplate);                
                await dbContext.SaveChangesAsync();
                response.Success();
            }
            return response;
        }

        /// <summary>
        /// 更新客戶印鑑樣板
        /// </summary>        
        /// <param name="customerSealTemplateUpdateForm">客戶印鑑樣板</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Update(CustomerSealTemplateUpdateForm customerSealTemplateUpdateForm)
        {
            ResponseViewModel response = new ();
            int userid = 1;
            CustomerSealTemplate? customerSealTemplateQuery = dbContext.CustomerSealTemplates.Include(x => x.CustomerSealTemplateLocations)                                                             
                                                                                             .FirstOrDefault(x => x.Id == customerSealTemplateUpdateForm.Id);

            if (customerSealTemplateQuery != null)
            {
                //儲存圖片(原圖)                
                await imageService.SaveImageAsync(customerSealTemplateUpdateForm.ImageBase64, customerSealTemplateQuery.ImageViewFullPath, false);
                //儲存縮圖                
                await imageService.SaveImageAsync(customerSealTemplateUpdateForm.ImageBase64Thumbnail, customerSealTemplateQuery.ThumbnailFullPath, false);

                mapper.Map(customerSealTemplateUpdateForm, customerSealTemplateQuery);
                BaseInputCustomerSealTemplate(customerSealTemplateQuery, false, userid);
                
                //刪除樣本座標
                foreach(int deleteLocationId in customerSealTemplateUpdateForm.DeleteLocationIds)
                {
                    CustomerSealTemplateLocation? customerSealTemplateLocation = customerSealTemplateQuery.CustomerSealTemplateLocations
                                                                                                          .FirstOrDefault(x => x.Id == deleteLocationId);
                    if(customerSealTemplateLocation != null)
                    {
                        dbContext.Remove(customerSealTemplateLocation);
                    }
                }
                //修改樣本座標
                foreach(CustomerSealTemplateLocationUpdateForm locationUpdateForm in customerSealTemplateUpdateForm.LocationUpdateForms)
                {
                    CustomerSealTemplateLocation? customerSealTemplateLocation = customerSealTemplateQuery.CustomerSealTemplateLocations
                                                                                                          .FirstOrDefault(x => x.Id == locationUpdateForm.Id);
                    if(customerSealTemplateLocation != null) 
                    {
                        mapper.Map(locationUpdateForm, customerSealTemplateLocation);
                    }
                }
                //新增樣本座標
                foreach (CustomerSealTemplateLocationForm locationForm in customerSealTemplateUpdateForm.LocationForms)
                {
                    customerSealTemplateQuery.CustomerSealTemplateLocations.Add(mapper.Map<CustomerSealTemplateLocation>(locationForm));
                }
                await dbContext.SaveChangesAsync();
                response.Success();
            }

            return response;
        }
        /// <summary>
        /// 刪除客戶印鑑樣板
        /// </summary>
        /// <param name="Id">客戶印鑑樣板Id</param>
        /// <returns></returns>
        public ResponseViewModel Delete (int Id)
        {
            ResponseViewModel response = new();
            int userId = 0;

            CustomerSealTemplate? customerSealTemplateQuery = dbContext.CustomerSealTemplates.Find(Id);

            if(customerSealTemplateQuery != null) 
            {
                customerSealTemplateQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputCustomerSealTemplate(customerSealTemplateQuery, false, userId);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteCustomerSealTemplateNoData();
            }

            return response;
        }

        /// <summary>
        /// 讀取分頁資料
        /// </summary>        
        /// <param name="customerSealTemplateQuery">樣板</param>
        /// <param name="pageNumber">頁次</param>
        /// <param name="pageSize">頁面大小</param>        
        private List<CustomerSealTemplateViewModel> LoadPaginatedData(IQueryable<CustomerSealTemplate> customerSealTemplateQuery,int pageNumber, int pageSize)
        {
            return 
                customerSealTemplateQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(customerSealTemplate => new CustomerSealTemplateViewModel()
                {
                    Id = customerSealTemplate.Id,
                    Name = customerSealTemplate.Name,
                    ImageFullPath = customerSealTemplate.ThumbnailFullPath,
                    ThumbnailBase64 = imageService.GetPathToBase64(customerSealTemplate.ThumbnailFullPath)
                }).ToList();
        }

        /// <summary>
        /// 紀錄分頁Log
        /// </summary>
        /// <param name="customerSealTemplatePaginate">分頁列表</param>
        private void SavePaginateLog(CustomerSealTemplatePaginate customerSealTemplatePaginate)
        {
            CustomerSealTemplatePaginateLog customerSealTemplatePaginateLog = mapper.Map<CustomerSealTemplatePaginateLog>(customerSealTemplatePaginate);
            customerSealTemplatePaginateLog.LogModels = mapper.Map<List<CustomerSealTemplateLogModel>>(customerSealTemplatePaginate.ViewModels);
            Log.Information("CustomerSealTemplate paginate output {@Output}", customerSealTemplatePaginateLog);
        }

        /// <summary>
        /// 資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="customerSealTemplate">DB上的樣板資料</param>
        /// <param name="isCreate">確認是否新增還是更新的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputCustomerSealTemplate(CustomerSealTemplate customerSealTemplate, bool isCreate, int userid)
        {
            if (isCreate)
            {
                customerSealTemplate.CreateUserId = userid;
                customerSealTemplate.CreateDate = DateTime.Now;
                customerSealTemplate.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                customerSealTemplate.UpdateUserId = userid;
                customerSealTemplate.UpdateDate = DateTime.Now;
            }
        }        
    }
}
