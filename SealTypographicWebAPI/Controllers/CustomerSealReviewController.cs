using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.CustomerSealReview;

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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public CustomerSealReviewViewModelResponse Get()
        {
            return new ();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSealReviewOneSearch"></param>
        /// <returns></returns>
        [HttpGet("/CustomerSealReviewOneSearch")]
        public CustomerSealReviewDetailResponse Get([FromQuery]CustomerSealReviewOneSearch customerSealReviewOneSearch)
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
