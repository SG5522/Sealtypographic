using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排板PDF搜尋
    /// </summary>
    public class TypographicPDFSearch : PaginateSearch
    {
        /// <summary>
        ///輸入客戶編號或名稱
        /// </summary>        
        [Required]
        public string CustomerKeyWord { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        [MaxLength(5)]        
        public string? Quarter { get; set; }

        /// <summary>
        /// 建檔狀態(審核狀態)
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }
}
