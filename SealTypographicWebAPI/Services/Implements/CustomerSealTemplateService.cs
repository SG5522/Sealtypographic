using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶印鑑樣板
    /// </summary>
    public class CustomerSealTemplateService : ICustomerSealTemplateService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageSharpService;
        private readonly IMapper mapper;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageSharpService"></param>
        public CustomerSealTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageSharpService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageSharpService = imageSharpService;
        }

        /// <summary>
        /// 新增客戶印鑑樣板
        /// </summary>
        /// <param name="customerSealTemplateForm"></param>
        /// <returns></returns>
        public ResponseViewModel New(CustomerSealTemplateForm customerSealTemplateForm)
        {
            ResponseViewModel response = new();            
            int userid = 0; //帳號驗證取得ID
            int companyId = 1; //公司ID

            //尋找公司並與客戶關聯
            Company? companyQuery = dbContext.Companys
                                    .Include(x => x.CustomerSealTemplates)
                                    .Select(x => new Company { Id = x.Id })
                                    .FirstOrDefault(x => x.Id == companyId);

            if (companyQuery != null) 
            {                
                CustomerSealTemplate customerSealTemplate = new ()
                {
                    Name = customerSealTemplateForm.Name,
                    PageSize = customerSealTemplateForm.PageSize,
                    PaperOrientation = customerSealTemplateForm.PaperOrientation,
                    StackMode = customerSealTemplateForm.StackMode,
                    StackShift = customerSealTemplateForm.StackShift,                                        
                };
                List<CustomerSealTemplateLocation> customerSealTemplateLocations = new();
                foreach (CustomerSealTemplateLocationForm customerSealTemplateLocationForm in customerSealTemplateForm.CustomerSealTemplateLocationForms)
                {
                    CustomerSealTemplateLocation customerSealTemplateLocation = new()
                    {
                        ConfigType = customerSealTemplateLocationForm.CustomerSealType,
                        Left = customerSealTemplateLocationForm.Left,
                        Top = customerSealTemplateLocationForm.Top,
                        Height = customerSealTemplateLocationForm.Height,
                        Width = customerSealTemplateLocationForm.Width
                    };
                    customerSealTemplateLocations.Add(customerSealTemplateLocation);
                }
                BaseInputCustomerSealTemplate(customerSealTemplate, true, userid);
                //customerSealTemplate.CustomerTempTemplateLocations = customerSealTemplateLocations;
                //companyQuery.CustomerSealTemplates.Add(customerSealTemplate);
                //dbContext.Entry(companyQuery).State = EntityState.Unchanged;
                //dbContext.SaveChanges();
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
