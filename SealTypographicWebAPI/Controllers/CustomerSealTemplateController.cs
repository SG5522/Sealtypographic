using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 客戶印鑑樣板管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSealTemplateController : ControllerBase
    {
        private readonly ICustomerSealTemplateService customerSealTemplateService;

        /// <summary>
        /// 建構 注入Service
        /// </summary>
        /// <param name="customerSealTemplateService"></param>
        public CustomerSealTemplateController(ICustomerSealTemplateService customerSealTemplateService)
        {
            this.customerSealTemplateService = customerSealTemplateService;
        }

        // GET: api/<CustomerSealTemplateController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CustomerSealTemplateController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 新增客戶印鑑樣板
        /// </summary>
        /// <param name="customerSealTemplateForm"></param>        
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New([FromForm]CustomerSealTemplateForm customerSealTemplateForm)
        {
            ResponseViewModel response = new ();
            try
            {                
                Log.Information("CustomerSealTemplate new input {@Input}", customerSealTemplateForm);
                response = await customerSealTemplateService.New(customerSealTemplateForm);
                Log.Information("CustomerSealTemplate new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate new error {@Error}", ex);
                response.DbError();
            }

            return response;
        }

        /// <summary>
        /// 更新客戶印鑑樣板
        /// </summary>
        /// <param name="id">樣板ID</param>
        /// <param name="customerSealTemplateForm">客戶樣板</param>
        [HttpPut("{id}")]
        public async Task<ResponseViewModel> Update(int id, [FromForm]CustomerSealTemplateForm customerSealTemplateForm)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealTemplate update input id {@id}", id);
                Log.Information("CustomerSealTemplate update input customerSealTemplateForm {@customerSealTemplateForm}", customerSealTemplateForm);
                response = await customerSealTemplateService.Update(id, customerSealTemplateForm);
                Log.Information("CustomerSealTemplate new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealTemplate new error {@Error}", ex);
                response.DbError();
            }

            return response;
        }

        // DELETE api/<CustomerSealTemplateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
