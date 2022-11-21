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
        /// 取得顧客印鑑季度表
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        /// <returns></returns>
        public CustomerSealQuarters GetCustomerSealQuarters(string customerId)
        {
            List<CustomerSealQuarter> customerSealQuarters = new ();
            Response response;
            var customerSealQuarterQuery = dbContext.CustomerSealJournals
                                           .Where(customerSealJournal => customerSealJournal.CustomerId == customerId)
                                           .Select(customerSealJournal => customerSealJournal.Quarter)
                                           .Distinct()
                                           .ToList();

            if (customerSealQuarterQuery.Any()) 
            {                
                foreach(string quarter in customerSealQuarterQuery)
                {
                    customerSealQuarters.Add(new CustomerSealQuarter
                    {
                        CustomerId = customerId,
                        Quarter = quarter
                    });
                }
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }

            return new CustomerSealQuarters()
            {

                Code = response.Code,
                Message = response.Message,

                Quarters = customerSealQuarters
            };
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarter">搜尋條件</param>
        /// <returns></returns>
        public CustomerSealViewModels GetCustomerSealViewModels(CustomerSealQuarter customerSealQuarter) 
        {
            List<CustomerSealViewModel> customerSealViewModels = new();
            Response response;
            var customerSealQuery = (from customerSealJournal in dbContext.Set<CustomerSealJournal>()
                                    join imageGroup in dbContext.Set<ImageGroup>()
                                    on customerSealJournal.ImageGroupId equals imageGroup.Id
                                    where customerSealJournal.CustomerId == customerSealQuarter.CustomerId
                                    && customerSealJournal.Quarter == customerSealQuarter.Quarter
                                    select new
                                    {
                                        customerSealJournal.CustomerId,
                                        customerSealJournal.ImageGroupId,
                                        ImageGroupName = imageGroup.Name,
                                        customerSealJournal.No,
                                        customerSealJournal.ImagePath,
                                        customerSealJournal.Quarter,
                                    })
                                    .OrderBy(customerSealJournal => customerSealJournal.ImageGroupId)
                                    .ToList();
                                    
            if (customerSealQuery.Any()) 
            {
                foreach(var customerSealJournal in customerSealQuery)
                {
                    customerSealViewModels.Add(new CustomerSealViewModel
                    {
                        CustomerId = customerSealJournal.CustomerId,
                        ImageGroupId = customerSealJournal.ImageGroupId,
                        ImageGroupName = customerSealJournal.ImageGroupName,
                        No = customerSealJournal.No,                        
                        ImageBase64 = customerSealJournal.ImagePath, //之後會在做BASE64轉換
                        Quarter = customerSealJournal.Quarter
                    });
                }
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }


            return new CustomerSealViewModels()
            {
                Code= response.Code,
                Message= response.Message,

                SealViewModels = customerSealViewModels
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
                //這段之後會做成IMAGE64的處理並另存在指定的位置
                string imagePath = customerSeal.ImageBase64;

                customerSealJournals.Add(new CustomerSealJournal()
                    {
                        CustomerId = customerSeal.CustomerId,
                        No = customerSeal.No,
                        ImageGroupId = customerSeal.ImageGroupId,
                        ImagePath = imagePath,
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
        public Response UpdateCustomerSeals(List<CustomerSealPostData> customerSeals)
        {
            Response response = new();
            foreach (CustomerSealPostData customerSeal in customerSeals)
            {
                var customerSealJournalQuery = dbContext.CustomerSealJournals.Where
                                           (
                                                customerSealJournal =>
                                                customerSealJournal.Id == customerSeal.Id
                                           );
                if(customerSealJournalQuery.Any())
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = customerSeal.ImageBase64;

                    CustomerSealJournal customerSealJournal = customerSealJournalQuery.First();
                    customerSealJournal.ImagePath = imagePath;
                    customerSealJournal.AvailableDate = customerSeal.AvailableDate;
                    customerSealJournal.No = customerSeal.No;
                    customerSealJournal.CreateDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                else
                {
                    response = responseService.Get(ResponseCode.NoData);
                    return response;
                }
            }
            response = responseService.Get(ResponseCode.Success);
            return response;
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
