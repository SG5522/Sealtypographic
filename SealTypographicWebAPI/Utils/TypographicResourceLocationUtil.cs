using DBEntities.Consts;
using DBEntities.Entities.TypographicModels;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// Db上TypographyResourceUtil的一些常用的新增與修改的參數
    /// </summary>
    public class TypographicResourceLocationUtil
    {
        /// <summary>
        /// 從資料庫參數中判斷該取得哪個ImageBase64(透通處理)
        /// </summary>        
        /// <param name="typographicResourceLocation">Db上的排版印鑑資料</param>
        /// <returns></returns>
        public static string GetImageBase64(TypographicResourceLocation typographicResourceLocation)
        {
            return ImageTransparentUtil.ToDataUrl(GetImagePath(typographicResourceLocation));
        }

        /// <summary>
        /// 從資料庫參數中判斷該取得哪個ImagePath
        /// </summary>        
        /// <param name="typographicResourceLocation">Db上的排版印鑑資料</param>
        public static string GetImagePath(TypographicResourceLocation typographicResourceLocation)
        {
            string result;
            //確認是否有排版中加入編輯後的印鑑
            if (string.IsNullOrWhiteSpace(typographicResourceLocation.EditImageFullPath))
            {
                result = typographicResourceLocation.TypographicResource.ImageFullPath;
            }
            else
            {
                result = typographicResourceLocation.EditImageFullPath;
            }
            return result;
        }
    }
}
