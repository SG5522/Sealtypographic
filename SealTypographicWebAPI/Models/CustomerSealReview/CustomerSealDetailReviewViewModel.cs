using DBEntities.Consts;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSeal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核詳細資料
    /// </summary>
    public class CustomerSealDetailReviewViewModel
    {
        /// <summary>
        /// new Seals
        /// </summary>
        public CustomerSealDetailReviewViewModel() 
        {
            Seals = new();
        }

        /// <summary>
        /// 印鑑季度Id
        /// </summary>         
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// 客戶編號(更新或搜尋使用)
        /// </summary>
        /// <example>AAA001</example>        
        public string Code { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>映像公司</example>        
        public string Name { get; set; }

        /// <summary>
        /// 顯示此筆季度
        /// </summary>         
        /// <example>1</example>
        public int QuarterYearId { get; set; }


        //============LogSave============//

        /// <summary>
        /// 客戶Id
        /// </summary>
        [JsonIgnore]
        public int CustomerId { get; set; }

        /// <summary>
        /// 排版類別
        /// </summary>
        [JsonIgnore]
        public TypographyType TypographyType { get; set; }

        /// <summary>
        /// 公曆年季度
        /// </summary>
        [JsonIgnore]
        public string GregorainQuarterYear { get; set; }

        /// <summary>
        /// 顯示用年季度(目前使用民國年)
        /// </summary>
        [JsonIgnore]
        public string DisplayQuarterYear { get; set; }

        //============LogSave============//

        /// <summary>
        /// 客戶印鑑組
        /// </summary>
        public List<CustomerSealViewModel> Seals { get; set; }
    }

}
