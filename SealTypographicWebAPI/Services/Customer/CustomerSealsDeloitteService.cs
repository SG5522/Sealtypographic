using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.DbModels;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services.Customer
{
    /// <summary>
    /// 勤業用的顧客印鑑組
    /// </summary>
    public class CustomerSealsDeloitteService : ICustomerSealService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ResponseService responseService;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="responseService"></param>
        public CustomerSealsDeloitteService(SealTypographicDbContext dbContext, ResponseService responseService)
        {
            this.dbContext = dbContext;
            this.responseService = responseService;
        }


        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        public CustomerSeals GetCustomerSeals(string customerID, string quarter)
        {
            List<CustomerSeal> customerSeals = new();

            for (int i = 0; i < 4; i++)
            {
                //測試資料
                CustomerSeal customerSeal = new()
                {
                    CustomerId = customerID,
                    ImageGroupId = i + 1,
                    No = 1,
                    ImageBase64 = "C://123.jpg",
                    AvailableDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Quarter = quarter,
                };
                customerSeals.Add(customerSeal);
            }
            return new CustomerSeals()
            {

                Code = 200,
                Message = "Success",

                Seals = customerSeals
            };
        }

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑組</param>
        /// <returns></returns>
        public Response CreateCustomerSeals(List<CustomerSeal> customerSeals)
        {
            Response response = new();
            List<CustomerSealJournal> customerSealJournals = new();
            foreach(CustomerSeal customerSeal in customerSeals)
            {
                customerSealJournals.Add(new CustomerSealJournal()
                    {
                        CustomerId = customerSeal.CustomerId,
                        No = customerSeal.No,
                        ImageGroupId = customerSeal.ImageGroupId,
                        ImagePath = "C:..",
                        CreateDate = DateTime.Now,
                        Quarter = customerSeal.Quarter,                        
                    }
                );                
            }
            dbContext.CustomerSealJournals.AddRange(customerSealJournals);
            dbContext.BulkSaveChanges();
            response = responseService.Get(ResponseCode.Success);

            return response;
        }

        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑組</param>
        /// <returns></returns>
        public Response UpdateCustomerSeals(List<CustomerSeal> customerSeals)
        {
            return new Response();
        }


        /// <summary>
        /// 刪除印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑資料</param>
        /// <returns></returns>
        public  Response DeleteCustomerSeals(List<CustomerSeal> customerSeals)
        {
            return new Response();
        }
    }
}
