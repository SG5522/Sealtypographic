using DBEntities.Consts;
using DBEntities.Entities.Base;
using DBEntities.Utils;

namespace DBEntities.Extensions
{
    public static class ReviewStatusExtension
    {
        /// <summary>
        /// 設定input的reviewStatus與相關欄位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reviewStatus">客戶印鑑、會計師簽印的狀態</param>
        /// <param name="input">包含有BaseReviewData的資料庫model(Entites)</param>
        /// <param name="userId">使用者Id</param>
        /// <param name="isCreate"></param>
        public static void Set<T>(this ReviewStatus reviewStatus, T input, int userId, bool isCreate = false) where T : BaseReviewData
        {
            // For Draft, Pending, and Reject, no additional action needed
            if (reviewStatus >= ReviewStatus.Disabled)
            {
                input.DeleteStatus = DeleteStatus.Yes;
            }
            else if (reviewStatus == ReviewStatus.Approval)
            {
                input.StartDate = DateTime.Now;
                input.EndDate = DateTime.Parse("9999/12/31");
            }
            // Set review date for all cases except Draft
            if (reviewStatus != ReviewStatus.Draft) input.ReviewDate = DateTime.Now;

            input.ReviewStatus = reviewStatus;
            InputUtil.Set(input, isCreate, userId);
        }
    }
}
