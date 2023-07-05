using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客戶樣板座標
    /// </summary>
    public class CustomerSealTemplateLocationViewModel : BaseLocationModel
    {
        /// <summary>
        /// 客戶印鑑類別
        /// </summary>
        public CustomerSealType CustomerSealType { get; set; }
    }

    /// <summary>
    /// 客戶印鑑樣本詳細
    /// </summary>
    public class CustomerSealTemplateDetailViewModel : BaseTemplateWithResponse
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public CustomerSealTemplateDetailViewModel() 
        {
            LocaltionViewModels = new ();
        }

        /// <summary>
        /// 樣板Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 樣板疊放方式
        /// </summary>
        public StackMode StackMode { get; set; }

        /// <summary>
        /// 樣板疊放位移
        /// </summary>
        public int StackShift { get; set; }

        /// <summary>        
        /// 客戶印鑑樣板座標
        /// </summary>
        public List<CustomerSealTemplateLocationViewModel> LocaltionViewModels{ get; set; }
    }
}
