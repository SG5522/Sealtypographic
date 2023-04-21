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
            Default = new TWAINDriver();
        }

        /// <summary>
        /// 預設驅動程式
        /// </summary>
        public TWAINDriver Default { get; set; }
    }
}
