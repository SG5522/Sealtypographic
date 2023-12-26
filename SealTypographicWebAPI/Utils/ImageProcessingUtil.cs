using DBEntities.Consts;
using DJImageCommonLib.Consts;
using DJImageLib.Extensions;
using DJImageLib.Utils;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 圖片白色底圖透通
    /// </summary>
    public class ImageProcessingUtil
    {
        /// <summary>
        /// 從圖檔路徑輸入
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="angle">旋轉角度</param>
        /// <param name="sealDyeing">染色</param>
        /// <param name="isInpaint">是否補點</param>
        /// <returns></returns>
        public static string FromPath(string fullPath, float? angle, SealDyeing? sealDyeing, bool? isInpaint)
        {
            return FromDataUrl(ImageUtil.ToDataUrlFromFilePath(fullPath), angle, sealDyeing, isInpaint);
        }

        /// <summary>
        /// 從DataUrl輸入
        /// </summary>
        /// <param name="dataUrl"></param>
        /// <param name="angle">旋轉角度</param>
        /// <param name="sealDyeing">染色</param>
        /// <param name="isInpaint">是否補點</param>
        /// <returns></returns>
        public static string FromDataUrl(string dataUrl, float? angle, SealDyeing? sealDyeing, bool? isInpaint)
        {                        
            return FromBytes(DataUrlUtil.GetBase64(dataUrl).ToBytes(), angle, sealDyeing, isInpaint);
        }

        /// <summary>
        /// 從Bytes輸入
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <param name="angle">旋轉角度</param>
        /// <param name="sealDyeing">染色</param>
        /// <param name="isInpaint">是否補點</param>
        /// <returns></returns>
        public static string FromBytes(byte[] srcBytes, float? angle, SealDyeing? sealDyeing, bool? isInpaint)
        {
            if(angle > 0f)
            {
                srcBytes = ImageUtil.Rotate(srcBytes, (float)angle);
            }
            if(sealDyeing > 0)
            {
                srcBytes = ImageUtil.RecolorToBytes(srcBytes, (Color)sealDyeing);
            }
            if((bool)isInpaint!)
            {
                srcBytes = ImageUtil.InpaintToBytes(srcBytes);
            }
            
            return ImageUtil.ToDataUrl(ImageUtil.Transparent(srcBytes, ImageConfigConsts.Threshold));
        }
        
    }
}
