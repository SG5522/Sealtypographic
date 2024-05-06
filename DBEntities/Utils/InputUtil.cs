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
        /// 設定為通過狀態
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        public static void SetReviewApproval<T>(T input, int userId) where T : BaseReviewData            
            => ReviewStatus.Approval.Set(input, userId);

        /// <summary>
        /// 設定為草稿狀態
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="isCreate"></param>
        /// <param name="userId"></param>
        public static void SetReviewDraft<T>(T input, int userId, bool isCreate = false) where T : BaseReviewData
            => ReviewStatus.Draft.Set(input, userId, isCreate);

        /// <summary>
        /// 設定為待審狀態
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        public static void SetReviewPending<T>(T input, int userId) where T : BaseReviewData
            => ReviewStatus.Pending.Set(input, userId);

        /// <summary>
        /// 設定為退件狀態
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        public static void SetReviewReject<T>(T input, int userId) where T : BaseReviewData
            => ReviewStatus.Reject.Set(input, userId);

        /// <summary>
        /// 設定為停用狀態
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        public static void SetReviewDisabled<T>(T input, int userId) where T : BaseReviewData
            => ReviewStatus.Disabled.Set(input, userId);

        /// <summary>
        /// 設定為作廢狀態
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        public static void SetReviewInvalid<T>(T input, int userId) where T : BaseReviewData
            => ReviewStatus.Invalid.Set(input, userId);

        /// <summary>
        /// 設定為不受理狀態
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        public static void SetReviewRefuse<T>(T input, int userId) where T : BaseReviewData
            => ReviewStatus.Refuse.Set(input, userId);

        /// <summary>
        /// 各類資料表的基本輸入處理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">輸入class</param>
        /// <param name="isCreate">是否為建立新表，否為Update</param>
        /// <param name="userId">使用者Id</param>
        public static void Set<T>(T input, bool isCreate, int userId) where T : BaseData
        {
            if (isCreate)
            {
                input.CreateUserId = userId;                
                input.CreateDate = DateTime.Now;
                input.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                input.UpdateUserId = userId;
                input.UpdateDate = DateTime.Now;
            }
        }
    }
}
