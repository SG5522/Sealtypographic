using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Service;
using SealTypographicWebAPI.Service.Accountant;
using SealTypographicWebAPI.Service.Letterhead;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 信頭資料處理
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LetterheadImageController : ControllerBase
    {
        /// <summary>
        /// 注入會計處理函式
        /// </summary>
        protected Letterhead letterhead = new(new LetterheadDeloitte());

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        protected ErrorMessage errorMessage = new();

        /// <summary>
        /// 取得信頭
        /// </summary>
        /// <param name="litterheadID"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int litterheadID)
        {
            try
            {
                List<LetterheadImageAddID> customerSeals = letterhead.GetLetterheadImages(litterheadID);
                return Ok(customerSeals);
            }
            catch
            {
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
            }
        }

        /// <summary>
        /// 建立信頭圖組
        /// </summary>
        /// <param name="letterheadImages"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(List<LetterheadImage> letterheadImages)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
            }
        }

        /// <summary>
        /// 修改信頭圖片組
        /// </summary>
        /// <param name="letterheadImages"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(List<LetterheadImageAddID> letterheadImages)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
            }
        }

        /// <summary>
        /// 刪除信頭
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
