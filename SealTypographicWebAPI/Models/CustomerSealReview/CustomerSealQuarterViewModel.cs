using NetTopologySuite.IO;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核
    /// </summary>
    public class CustomerSealQuarterViewModel : BaseName
    {
        /// <summary>
        /// New SealImageInfos
        /// </summary>
        public CustomerSealQuarterViewModel()
        {
            SealImageInfos = new();
        }

        /// <summary>
        /// 客戶編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// 印鑑資訊
        /// </summary>
        public List<SealImageInfo> SealImageInfos { get; set; }
    }

    /// <summary>
    /// 印鑑資訊
    /// </summary>
    public class SealImageInfo 
    {
        /// <summary>
        /// 印鑑類別
        /// </summary>
        public CustomerSealType CustomerSealType { get; set; }

        /// <summary>
        /// 印鑑編號(排序)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 縮圖字串(Base64)
        /// </summary>
        public string ThumbnailBase64 { get; set; }
    }

    /// <summary>
    /// 審核列表(分頁)
    /// </summary>
    public class CustomerSealQuarterResponse : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public CustomerSealQuarterResponse()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 審核LIST
        /// </summary>
        public List<CustomerSealQuarterViewModel> ViewModels { get; set; }
    }
}
