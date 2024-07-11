using DBEntities.Consts;
using DBEntities.Entities.Base;
using DBEntities.Extensions;

namespace DBEntities.Utils
{
    /// <summary>
    /// 各類資料的基本輸入
    /// </summary>
    public static class InputUtil
    {
        /// <summary>
        /// 各類資料表的基本輸入處理
        /// 新增時加入 CreateUserId、CreateDate、DeleteStatus
        /// 更新時加入 UpdateUserId、UpdateDate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">輸入class</param>
        /// <param name="isCreate">是否為建立新表，否為Update</param>
        /// <param name="userId">使用者Id</param>
        public static void Set<T>(T input, int userId, bool isCreate = false) where T : BaseData
        {
            if (isCreate)
            {
                input.CreateUserId = userId;                
                input.CreateDate = DateTimeOffset.Now;
                input.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                input.UpdateUserId = userId;
                input.UpdateDate = DateTimeOffset.Now;
            }
        }

        /// <summary>
        /// 設定為停用
        /// 預設是系統預設人員停用 所預設userId = 1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">輸入class</param>
        /// <param name="userId">使用者Id(預設為1)</param>
        public static void SetDisabled<T>(T input, int userId = 1) where T : BaseReviewData
        {
            ReviewStatus.Disabled.Set(input, userId);
            Set(input, userId);
        }

        /// <summary>        
        /// 新增時的任何包含審核內容時的預設處理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">輸入class</param>
        /// <param name="userId">使用者Id(預設為1)</param>
        public static void SetDraftWithCreate<T>(T input, int userId) where T : BaseReviewData
        {
            //設定草稿狀態
            ReviewStatus.Draft.Set(input, userId);
            //基本輸入處理
            Set(input, userId, true);
        }
    }
}
