using TWAINWorkingGroup;

namespace DJScannerLib.Models
{
    /// <summary>
    /// 預設Driver結果
    /// </summary>
    public class DefaultDriverResult : BaseResult
    {
        public DefaultDriverResult() 
        {
            Success = false;
            Default = string.Empty;
        }

        /// <summary>
        /// 預設驅動程式CSV字串
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// 預設驅動程式CSV字串[11]
        /// </summary>
        public string DefaultDriver 
        {
            get 
            {
                string[] identity = CSV.Parse(Default);
                return identity[11];
            }
        }
    }
}
