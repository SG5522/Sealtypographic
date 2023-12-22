using DJImageLib.Extensions;
using DJImageLib.Utils;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 紀錄base64圖檔使用
    /// </summary>
    public abstract class BaseImageBase64LogData : BaseData
    {
        private string imageBase64;

        /// <summary>
        /// ImageBase64字串
        /// </summary>
        public string ImageBase64
        {
            get { return imageBase64; }
            set
            {
                imageBase64 = value;
                if (imageBase64 != string.Empty)
                {                
                    
                    imageBase64 = ImageUtil.ReSize(DataUrlUtil.GetBase64(imageBase64).ToBytes(), 0.3, 0.3);
                }
            }
        }
    }
}
