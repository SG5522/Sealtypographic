using DJImageLib.Extensions;
using DJImageLib.Models;
using DJImageLib.Utils;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 圖片白色底圖透通
    /// </summary>
    public class ImageTransparentUtil
    {
        /// <summary>
        /// 從圖檔路徑輸入
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string FromPath(string fullPath)
        {
            return FromDataUrl(ImageUtil.ToDataUrlFromFilePath(fullPath));
        }

        /// <summary>
        /// 從DataUrl輸入
        /// </summary>
        /// <param name="dataUrl"></param>
        /// <returns></returns>
        public static string FromDataUrl(string dataUrl)
        {                        
            return FromBytes(DataUrlUtil.GetBase64(dataUrl).ToBytes());
        }

        /// <summary>
        /// 從Bytes輸入
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <returns></returns>
        public static string FromBytes(byte[] srcBytes)
        {            
            return ImageUtil.ToDataUrl(ImageUtil.Transparent(srcBytes, ImageConfigConsts.Threshold));
        }
        
    }
}
