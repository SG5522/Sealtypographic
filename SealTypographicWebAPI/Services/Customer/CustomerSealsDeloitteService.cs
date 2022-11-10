using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Customer
{
    /// <summary>
    /// 勤業用的顧客印鑑組
    /// </summary>
    public class CustomerSealsDeloitteService : ICustomerSealService
    {
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
                    GroupsId = i + 1,
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
            return new Response();
        }

        
    }
}
