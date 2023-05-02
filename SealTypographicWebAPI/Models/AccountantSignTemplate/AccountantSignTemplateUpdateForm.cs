using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantSignTemplate
{
    /// <summary>
    /// 會計師簽印樣板(分頁)
    /// </summary>
    public class AccountantSignTemplateUpdateForm : BaseTemplateWithFile
    {
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
        /// 刪除樣板位置座標
        /// </summary>                       
        public List<int> DeleteLocationIds { get; set; }

        /// <summary>
        /// 修改樣板位置座標
        /// </summary>                       
        public List<AccountantSignTemplateLocationUpdateForm> LocationUpdateForms { get; set; }

        /// <summary>
        /// 新增樣板位置座標
        /// </summary>                       
        public List<AccountantSignTemplateLocationForm> LocationForms { get; set; }
    }
}
