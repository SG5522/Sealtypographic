using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class DeleteStatusUtil
    {        
        /// <summary>
        /// 待審
        /// </summary>
        /// <returns></returns>
        public static string NO()
        {
            return Get(DeleteStatus.NO);
        }

        /// <summary>
        /// 通過(審核完成)
        /// </summary>
        /// <returns></returns>
        public static string Yes()
        {
            return Get(DeleteStatus.Yes);
        }

        /// <summary>
        /// 取得Status名稱
        /// </summary>
        /// <returns></returns>
        public static string Get(DeleteStatus deleteStatus)
        {
            return deleteStatus switch
            {
                DeleteStatus.NO => "無標記",
                DeleteStatus.Yes => "隱藏",             
                _ => "",
            };
        }
    }
}
