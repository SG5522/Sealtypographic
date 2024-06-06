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
        public static string ToDataUrl(string fullPath)
        {
            return ToDataUrlFromDataUrl(ImageUtil.ToDataUrlFromFilePath(fullPath));
        }

        /// <summary>
        /// 從Base64輸入
        /// </summary>
        /// <param name="imageBase64"></param>
        /// <returns>回傳DataUrl</returns>
        public static string ToDataUrlFromImageBase64(string imageBase64)
        {
            return ToDataUrl(imageBase64.ToBytes());
        }

        /// <summary>
        /// 從DataUrl輸入
        /// </summary>
        /// <param name="dataUrl"></param>
        /// <returns>回傳DataUrl</returns>
        public static string ToDataUrlFromDataUrl(string dataUrl)
        {                        
            return ToDataUrl(DataUrlUtil.GetBase64(dataUrl).ToBytes());
        }

        /// <summary>
        /// 從Bytes輸入
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <returns></returns>
        public static string ToDataUrl(byte[] srcBytes)
        {            
            return ImageUtil.ToDataUrl(ImageUtil.Transparent(srcBytes, ImageConfigConsts.Threshold));
        }
        
    }
}
