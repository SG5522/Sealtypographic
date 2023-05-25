using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
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

            Template? templateQuery = dbContext.Templates
                                                .Include(x => x.TemplateLocations)
                                                .FirstOrDefault(x => x.Id == Id);

            if(templateQuery != null) 
            {
                customerSealTemplateDetailViewModel = mapper.Map<CustomerSealTemplateDetailViewModel>(templateQuery);
                customerSealTemplateDetailViewModel.LocaltionViewModels = mapper.Map<List<CustomerSealTemplateLocationViewModel>>(templateQuery.TemplateLocations);
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

            string? imagePath = dbContext.Templates.Where(x => x.Id == id)
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
                int totalPage = templateQuery.Count();
                customerSealTemplatePaginate.ViewModels = LoadPaginatedData(templateQuery, customerSealTemplateSearch.PageNumber, customerSealTemplateSearch.PageSize);
                customerSealTemplatePaginate.PageNumber = customerSealTemplateSearch.PageNumber;
                customerSealTemplatePaginate.PageSize = customerSealTemplateSearch.PageSize;
                //計算總頁數
                customerSealTemplatePaginate.TotalPage = TotalPageUtil.GetTotalPage(totalPage, customerSealTemplateSearch.PageSize);
                customerSealTemplatePaginate.TotalCount = totalPage;
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
                                    .Include(x => x.Templates)
                                    .FirstOrDefault(x => x.Id == companyId);

            if (companyQuery != null) 
            {                
                Template template = mapper.Map<Template>(customerSealTemplateForm);
                List<TemplateLocation> templateLocations = new();
                //儲存圖片(原圖)
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, (SealType)SealType.Customer);
                imageBase64Info.ImageBase64 = customerSealTemplateForm.ImageBase64;
                template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                //儲存縮圖
                imageBase64Info.ImageBase64 = customerSealTemplateForm.ImageBase64Thumbnail;
                template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);
                
                NewTemplateLoction(customerSealTemplateForm.CustomerSealTemplateLocationForms, templateLocations);
                BaseInputCustomerSealTemplate(template, true, userid);
                template.TemplateLocations = templateLocations;                
                companyQuery.Templates.Add(template);
                                  
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
            Template? templateQuery = dbContext.Templates.Include(x => x.TemplateLocations)                                                             
                                                         .FirstOrDefault(x => x.Id == customerSealTemplateUpdateForm.Id);

            if (templateQuery != null)
            {
                //儲存圖片(原圖)                
                await imageService.SaveImageAsync(customerSealTemplateUpdateForm.ImageBase64, templateQuery.ImageViewFullPath, false);
                //儲存縮圖                
                await imageService.SaveImageAsync(customerSealTemplateUpdateForm.ImageBase64Thumbnail, templateQuery.ThumbnailFullPath, false);

                mapper.Map(customerSealTemplateUpdateForm, templateQuery);
                BaseInputCustomerSealTemplate(templateQuery, false, userid);
                
                //刪除樣本座標
                foreach(int deleteLocationId in customerSealTemplateUpdateForm.DeleteLocationIds)
                {
                    TemplateLocation? templateLocation = templateQuery.TemplateLocations.FirstOrDefault(x => x.Id == deleteLocationId);
                    if(templateLocation != null)
                    {
                        templateQuery.TemplateLocations.Remove(templateLocation);                        
                    }
                }
                //修改樣本座標
                foreach(CustomerSealTemplateLocationUpdateForm locationUpdateForm in customerSealTemplateUpdateForm.LocationUpdateForms)
                {
                    TemplateLocation? templateLocation = templateQuery.TemplateLocations.FirstOrDefault(x => x.Id == locationUpdateForm.Id);

                    if(templateLocation != null) 
                    {
                        mapper.Map(locationUpdateForm, templateLocation);
                        templateLocation.SealType = SealType.Customer;
                        //之後要調整為不用轉型
                        templateLocation.SubSealType = SealMappingConfigUtil.GetSubSealTypeWithCustomer((CustomerSealType)locationUpdateForm.CustomerSealType);                        
                    }
                }
                //新增樣本座標
                NewTemplateLoction(customerSealTemplateUpdateForm.LocationForms, templateQuery.TemplateLocations);

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

            Template? templateQuery = dbContext.Templates.Find(Id);

            if(templateQuery != null) 
            {
                templateQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputCustomerSealTemplate(templateQuery, false, userId);
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
        /// <param name="templateQuery">樣板</param>
        /// <param name="pageNumber">頁次</param>
        /// <param name="pageSize">頁面大小</param>        
        private List<CustomerSealTemplateViewModel> LoadPaginatedData(IQueryable<Template> templateQuery,int pageNumber, int pageSize)
        {
            return 
                templateQuery
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
        /// <param name="template">DB上的樣板資料</param>
        /// <param name="isCreate">確認是否新增還是更新的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputCustomerSealTemplate(Template template, bool isCreate, int userid)
        {
            if (isCreate)
            {
                template.CreateUserId = userid;
                template.CreateDate = DateTime.Now;
                template.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                template.UpdateUserId = userid;
                template.UpdateDate = DateTime.Now;
            }
        }      
        
        /// <summary>
        /// 新增樣版位置
        /// </summary>
        /// <param name="customerSealTemplateLocationForms"></param>
        /// <param name="templateLocations"></param>
        private void NewTemplateLoction(List<CustomerSealTemplateLocationForm> customerSealTemplateLocationForms, List<TemplateLocation> templateLocations)
        {
            foreach (CustomerSealTemplateLocationForm customerSealTemplateLocationForm in customerSealTemplateLocationForms)
            {
                TemplateLocation templateLocation = mapper.Map<TemplateLocation>(customerSealTemplateLocationForm);
                templateLocation.SealType = SealType.Customer;
                //之後要調整為不用轉型
                templateLocation.SubSealType = SealMappingConfigUtil.GetSubSealTypeWithCustomer((CustomerSealType)customerSealTemplateLocationForm.CustomerSealType);
                templateLocations.Add(templateLocation);
            }
        }
    }
}
