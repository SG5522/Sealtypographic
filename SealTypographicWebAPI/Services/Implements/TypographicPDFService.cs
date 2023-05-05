using AutoMapper;
using DBEntities;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 管理PDF排版資訊
    /// </summary>
    public class TypographicPDFService : ITypographicPDFService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與Automapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public TypographicPDFService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFSearch">搜尋條件</param>
        /// <returns></returns>
        public TypographicPDFViewModel GetTypographicPDFViewModel(TypographicPDFSearch typographicPDFSearch)
        {

            return new();
        }

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="pDFId">上傳檔案的Id</param>
        /// <param name="customerId"></param>        
        /// <returns></returns>
        public ResponseViewModel New(int pDFId, int customerId)
        {
            ResponseViewModel response = new();            
            int userId = 0;
            
            UploadFile? pDFInfo = dbContext.UploadFiles.Find(pDFId);            
            
            if (pDFInfo != null)
            {
                Customer? customer = dbContext.Customers.Include(x => x.TypographicPDFs).FirstOrDefault(x => x.Id == customerId);
                if(customer != null)
                {

                    TypographicPDF typographicPDF = new()
                    {
                        OriginFileName = pDFInfo.OriginalFileName,
                        FullPath = pDFInfo.FullPath
                    };
                    BaseInputTypographicPDF(typographicPDF, true, userId);
                    customer.TypographicPDFs.Add(typographicPDF);
                }                
                
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DbNoData();
            }

            return response;
        }

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm"></param>
        /// <returns></returns>
        public ResponseViewModel Save(TypographicPDFForm typographicPDFForm)
        {
            return new();
        }

        private List<TypographicPage> GetTypographicPages(List<TypographicPageForm> typographicPageForms)
        {
            List<TypographicPage> typographicPages = new();
            foreach (TypographicPageForm typographicPageForm in typographicPageForms)
            {
                typographicPages.Add(mapper.Map<TypographicPage>(typographicPageForm));
            }
            return typographicPages;
        }

        private List<CustomerSealLocation> GetCusTomerSealLocations(List<CustomerSealLocationForm> customerSealLocationForms)
        {
            List<CustomerSealLocation> customerSealLocations = new();
            foreach (CustomerSealLocationForm customerSealLocationForm in customerSealLocationForms)
            {
                customerSealLocations.Add(mapper.Map<CustomerSealLocation>(customerSealLocationForm));
            }
            return customerSealLocations;
        }

        /// <summary>
        /// 資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="typographicPDF">DB上的排板PDF資料</param>        
        /// <param name="isCreate">確認是否新增還是更新的動作</param>
        /// <param name="userid">使用者ID</param>
        private void BaseInputTypographicPDF(TypographicPDF typographicPDF, bool isCreate, int userid)
        {
            if (isCreate)
            {
                typographicPDF.Quarter = "unassigned";
                typographicPDF.CreateUserId = userid;
                typographicPDF.CreateDate = DateTime.Now;
                typographicPDF.DeleteStatus = DeleteStatus.No;
                typographicPDF.ReviewStatus = ReviewStatus.Draft;
            }
            else
            {
                typographicPDF.UpdateUserId = userid;
                typographicPDF.UpdateDate = DateTime.Now;
            }
        }            
    }
}
