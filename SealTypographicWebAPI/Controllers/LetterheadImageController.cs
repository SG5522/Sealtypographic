using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Util;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 信頭資料處理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LetterheadImageController : ControllerBase
    {
        /// <summary>
        /// 宣告信頭的interface
        /// </summary>
        protected readonly ILetterheadService letterheadService;

        /// <summary>
        /// 注入信頭interface
        /// </summary>
        /// <param name="letterheadService"></param>
        public LetterheadImageController(ILetterheadService letterheadService)
        {
            this.letterheadService = letterheadService;
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        protected ResponseUtil responseService = new();

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
                return Ok();
            }
            catch
            {
                return NotFound(ResponseUtil.DBError());
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
                return NotFound(ResponseUtil.DBError());
            }
        }

        /// <summary>
        /// 修改信頭圖片組
        /// </summary>
        /// <param name="letterheadImages"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(List<LetterheadImageWithId> letterheadImages)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                return NotFound(ResponseUtil.DBError());
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
