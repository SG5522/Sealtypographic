using TWAINWorkingGroup;

namespace DJScannerLib.Models
{
    /// <summary>
    /// TWAIN 驅動程式
    /// </summary>
    public class TWAINDriver
    {

        /// <summary>
        /// 驅動程式CSV字串
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 驅動程式CSV字串[11]
        /// </summary>
        public string DriverName
        {
            get
            {
                string[] identity = CSV.Parse(Identity);
                return identity[11];
            }
        }
    }
}
