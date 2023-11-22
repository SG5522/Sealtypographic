using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 圖片截取範圍設定
    /// </summary>
    public class Setting : BaseData
    {
        /// <summary>
        /// 
        /// </summary>
        public SettingOption SettingOption { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company? Company { get; set; }

    }
}
