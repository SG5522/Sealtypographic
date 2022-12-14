using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 客戶印鑑審核
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSealReviewController : ControllerBase
    {
        /// <summary>
        /// 客戶印鑑審核管理的interface
        /// </summary>
        protected readonly ICustomerSealReviewService customerSealReviewService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="customerSealReviewService">客戶印鑑審核管理</param>
        public CustomerSealReviewController(ICustomerSealReviewService customerSealReviewService)
        {
            this.customerSealReviewService = customerSealReviewService;
        }

        /// <summary>
        /// 客戶印鑑待審清單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public CustomerSealReviewViewModelResponse Get([FromQuery]CustomerSealReviewSearch customerSealReviewSearch)
        {
            CustomerSealReviewViewModelResponse customerSealReviewViewModelResponse = new ();
            customerSealReviewViewModelResponse = customerSealReviewService.GetCustomerSealReviewViewModel(customerSealReviewSearch);

            return customerSealReviewViewModelResponse;
        }

        /// <summary>
        /// 基本資料與單季所有印鑑
        /// </summary>
        /// <param name="customerSealReviewQuarterSearch"></param>
        /// <returns></returns>
        [HttpGet("/customerSealReviewQuarterSearch")]
        public CustomerSealReviewDetailResponse Get([FromQuery]CustomerSealReviewQuarterSearch customerSealReviewQuarterSearch)
        {
            return new();
        }

        // POST api/<CustomerReviewController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerReviewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerReviewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
