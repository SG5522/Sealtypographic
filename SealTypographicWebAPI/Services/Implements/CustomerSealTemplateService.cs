using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Utils;
using Serilog;
using Serilog.Parsing;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶印鑑樣板
    /// </summary>
    public class CustomerSealTemplateService : ICustomerSealTemplateService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageSharpService;
        private readonly TemplateImagePathOption templateImagePathOption;
        private readonly IMapper mapper;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageSharpService"></param>
        /// <param name="option"></param>
        public CustomerSealTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageSharpService, IOptionsSnapshot<TemplateImagePathOption> option)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageSharpService = imageSharpService;
            this.templateImagePathOption = option.Value;
        }

        /// <summary>
        /// 客戶印鑑樣板分頁顯示
        /// </summary>
        /// <param name="customerSealTemplateSearch"></param>
        /// <returns></returns>
        public CustomerSealTemplatePaginate Paginate(CustomerSealTemplateSearch customerSealTemplateSearch)
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
                List<CustomerSealTemplateViewModel> thisPageCustomerSealTemplate = customerSealTemplateQuery                                                                      
                                                                    .Skip((customerSealTemplateSearch.PageNumber - 1) * customerSealTemplateSearch.PageSize)
                                                                    .Take(customerSealTemplateSearch.PageSize)
                                                                    .Select(customerSealTemplate => new CustomerSealTemplateViewModel()
                                                                    {
                                                                        Id = customerSealTemplate.Id,
                                                                        Name = customerSealTemplate.Name,
                                                                        ImageFullPath = customerSealTemplate.ThumbnailFullPath,
                                                                        ThumbnailBase64 = imageSharpService.GetPathToBase64(customerSealTemplate.ThumbnailFullPath)
                                                                    })
                                                                    .ToList();

                customerSealTemplatePaginate.ViewModels = thisPageCustomerSealTemplate;
                customerSealTemplatePaginate.PageNumber = customerSealTemplateSearch.PageNumber;
                customerSealTemplatePaginate.PageSize = customerSealTemplateSearch.PageSize;
                //計算總頁數
                customerSealTemplatePaginate.TotalPage = TotalPageUtil.GetTotalPage(customerSealTemplateQuery.Count(), customerSealTemplateSearch.PageSize);
                customerSealTemplatePaginate.TotalCount = customerSealTemplateQuery.Count();
                customerSealTemplatePaginate.Success();
            }
            CustomerSealTemplatePaginateLog customerSealTemplatePaginateLog = mapper.Map<CustomerSealTemplatePaginateLog>(customerSealTemplatePaginate);
            customerSealTemplatePaginateLog.LogModels = mapper.Map<List<CustomerSealTemplateLogModel>>(customerSealTemplatePaginate.ViewModels);            
            Log.Information("CustomerSealTemplate paginate output {@Output}", customerSealTemplatePaginate);
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
                customerSealTemplate.ImageViewFullPath = await FormFileUtil.UploadFileReturnPath(customerSealTemplateForm.ImageView, companyQuery.Code, templateImagePathOption.Customer);
                customerSealTemplate.ThumbnailFullPath = await FormFileUtil.UploadFileReturnPath(customerSealTemplateForm.Thumbnail, companyQuery.Code, templateImagePathOption.Customer);
                List<CustomerSealTemplateLocation> customerSealTemplateLocations = new();
                foreach (CustomerSealTemplateLocationForm customerSealTemplateLocationForm in customerSealTemplateForm.CustomerSealTemplateLocationForms)
                {                   
                    customerSealTemplateLocations.Add(mapper.Map<CustomerSealTemplateLocation>(customerSealTemplateLocationForm));                    
                }
                BaseInputCustomerSealTemplate(customerSealTemplate, true, userid);
                customerSealTemplate.CustomerTempTemplateLocations = customerSealTemplateLocations;                
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
        /// <param name="customerSealTemplateUpdateForm">客戶樣板</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Update(CustomerSealTemplateUpdateForm customerSealTemplateUpdateForm)
        {
            ResponseViewModel response = new ();
            CustomerSealTemplate? customerSealTemplateQuery = dbContext.CustomerSealTemplates.Include(x => x.CustomerTempTemplateLocations)
                                                             .Select
                                                             (
                                                                x => new CustomerSealTemplate()
                                                                {
                                                                    Id = x.Id,
                                                                    CustomerTempTemplateLocations = x.CustomerTempTemplateLocations,
                                                                }
                                                             )
                                                             .FirstOrDefault(x => x.Id == customerSealTemplateUpdateForm.Id);


            if (customerSealTemplateQuery != null)
            {
                response.Success();
            }

            return response;
        }

        /// <summary>
        /// 資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="customerSealTemplate">DB上的客戶資料</param>
        /// <param name="isCreate">確認是否新增的動作</param>
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
