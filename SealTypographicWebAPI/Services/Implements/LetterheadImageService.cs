using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 信頭圖片管理
    /// </summary>
    public class LetterheadImageService : ILetterheadImageService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        public LetterheadImageService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得信頭圖片群組創建日期列表
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        public LetterheadGroupCreateDateViews GetLetterheadGroupCreateDateViews(int letterheadId)
        {
            LetterheadGroupCreateDateViews letterheadGroupCreateDateViews = new();
            List<LetterheadImageGroupCreateDateView> groupCreateDateViews = dbContext.LetterheadImageJournals
                                           .Where
                                           (
                                                letterheadImageJournal => letterheadImageJournal.LetterheadId == letterheadId
                                                && letterheadImageJournal.ReviewStatus <= ReviewStatus.Draft //草稿狀態以下顯示
                                           )
                                           .Select(letterheadImageJournal => new LetterheadImageGroupCreateDateView()
                                           {
                                               LetterheadId = letterheadImageJournal.LetterheadId,
                                               GroupCreateDate = letterheadImageJournal.GroupCreateDate,
                                               ReviewStatus = letterheadImageJournal.ReviewStatus
                                           })
                                           .GroupBy(letterheadImageJournal => letterheadImageJournal.GroupCreateDate)
                                           .OrderByDescending(g => g.Key)
                                           .Select(letterheadImageJournal => letterheadImageJournal.First())
                                           .ToList();

            if (groupCreateDateViews.Any())
            {
                letterheadGroupCreateDateViews.GroupCreateDateViews = groupCreateDateViews;
                letterheadGroupCreateDateViews.Success();
            }
            else
            {
                letterheadGroupCreateDateViews.LetterheadImageNoData();
            }
            return letterheadGroupCreateDateViews;
        }

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="letterheadGroupCreateDateSearch">搜尋條件</param>
        /// <returns></returns>
        public LetterheadImageViewModels GetLetterheadImages (LetterheadImageGroupCreateDateSearch letterheadGroupCreateDateSearch)
        {
            LetterheadImageViewModels letterheadImageViewModels = new();
            List<LetterheadImageViewModel> ImageViewModels = new();

            letterheadImageViewModels.LetterheadId = letterheadGroupCreateDateSearch.LetterheadId;
            letterheadImageViewModels.GroupCreateDate = letterheadGroupCreateDateSearch.GroupCreateDate;
            List<LetterheadImageJournal> letterheadImageQuery = dbContext.LetterheadImageJournals.Where
                                                                    (
                                                                        letterheadImageJournal => letterheadImageJournal.LetterheadId == letterheadGroupCreateDateSearch.LetterheadId
                                                                        && letterheadImageJournal.GroupCreateDate == letterheadGroupCreateDateSearch.GroupCreateDate
                                                                        && letterheadImageJournal.DeleteStatus == DeleteStatus.NO
                                                                    )
                                                                    .Include(letterheadImageJournal => letterheadImageJournal.SealMappingConfig)
                                                                    .OrderBy(letterheadImageJournal => letterheadImageJournal.SealMappingConfigId)
                                                                    .ThenBy(letterheadImageJournal => letterheadImageJournal.Sequence)
                                                                    .ToList();
            if (letterheadImageQuery.Any())
            {
                foreach (LetterheadImageJournal letterheadImageJournal in letterheadImageQuery)
                {
                    LetterheadImageViewModel letterheadImageViewModel = mapper.Map<LetterheadImageViewModel>(letterheadImageJournal);
                    letterheadImageViewModel.ImageBase64 = letterheadImageJournal.ImagePath; //之後會在做BASE64轉換

                    ImageViewModels.Add(letterheadImageViewModel);
                }
                letterheadImageViewModels.ReviewStatus = letterheadImageQuery.First().ReviewStatus;
                letterheadImageViewModels.ImageViewModels = ImageViewModels;
                letterheadImageViewModels.Success();
            }
            else
            {
                letterheadImageViewModels.LetterheadImageNoData();
            }

            return letterheadImageViewModels;            
        }

        /// <summary>
        /// 新增圖片組
        /// </summary>
        /// <param name="letterheadImageForms">信頭圖片組</param>
        /// <returns></returns>
        public ResponseViewModel CreateLetterheadImages(List<LetterheadImageForm> letterheadImageForms)
        {
            ResponseViewModel response = new();
            List<LetterheadImageJournal> letterheadImageJournals = new();
            int userId = 0; //以後從帳號驗證取得Id

            LetterheadImageJournal? letterheadImageJournalQuery = dbContext.LetterheadImageJournals
                                                        .FirstOrDefault
                                                        (
                                                            x => x.LetterheadId == letterheadImageForms.First().LetterheadId                                                                                                
                                                            && x.ReviewStatus >= ReviewStatus.Draft
                                                            && x.ReviewStatus <= ReviewStatus.Pending
                                                        );
            if (letterheadImageJournalQuery == null)
            {
                DateTime createNowTime = DateTime.Now;//將建立日期為依據將此次建立的會計師簽印組成為一組Group
                foreach (LetterheadImageForm letterheadImageForm in letterheadImageForms)
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = letterheadImageForm.ImageBase64;

                    LetterheadImageJournal letterheadImageJournal = mapper.Map<LetterheadImageJournal>(letterheadImageForm);
                    letterheadImageJournal.ImagePath = imagePath;
                    letterheadImageJournal.GroupCreateDate = createNowTime;
                    BaseInputLetterheadImageJournal(letterheadImageJournal, true, userId);
                    letterheadImageJournals.Add(letterheadImageJournal);                    
                }
                dbContext.LetterheadImageJournals.AddRange(letterheadImageJournals);
                dbContext.BulkSaveChanges();
                response.Success();
            }
            else
            {
                response.LetterheadImageHaveDraftReviewStatus();
            }
            return response;
        }

        /// <summary>
        /// 異動會計師簽印的處理(審查狀態退回或是草稿才進行修改)
        /// </summary>
        /// <param name="letterheadImageUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        public List<ResponseViewModel> UpdateLetterheadImage(LetterheadImageUpdate letterheadImageUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();
            List<LetterheadImageJournal> letterheadImageJournals = new();
            int userId = 0;//之後會從帳號驗證中取得userid
            //刪除印鑑
            foreach (int letterheadImageId in letterheadImageUpdate.DeleteLetterheadImageIds)
            {
                LetterheadImageJournal? deleteLetterheadimageQuery = dbContext.LetterheadImageJournals.FirstOrDefault
                                                                (
                                                                    letterheadImage => letterheadImage.Id == letterheadImageId
                                                                    && letterheadImage.DeleteStatus == DeleteStatus.NO
                                                                    && letterheadImage.ReviewStatus <= ReviewStatus.Draft
                                                                );
                if (deleteLetterheadimageQuery != null)
                {
                    deleteLetterheadimageQuery.DeleteStatus = DeleteStatus.Yes;
                    deleteLetterheadimageQuery.UpdateDate = DateTime.Now;
                    deleteLetterheadimageQuery.UpdateUserId = userId;                    
                }
                else
                {
                    ResponseViewModel response = new();
                    response.DeleteAccountantSignNoData();
                    response.ErrorItem = "Delete letterheadImageJournalId:" + letterheadImageId;
                    responseViewModels.Add(response);
                }
            }
            //修改信頭圖像
            foreach (LetterheadImageFormUpdate letterheadImageFormUpdate in letterheadImageUpdate.UpdateLetterheadImages)
            {
                LetterheadImageJournal? letterheadImageJournal = dbContext.LetterheadImageJournals.FirstOrDefault
                                                            (
                                                                letterheadImage => letterheadImage.Id == letterheadImageFormUpdate.Id
                                                                && letterheadImage.DeleteStatus == DeleteStatus.NO
                                                                && letterheadImage.ReviewStatus <= ReviewStatus.Draft
                                                            );
                if (letterheadImageJournal != null)
                {
                    mapper.Map(letterheadImageFormUpdate, letterheadImageJournal);
                    string imagePath = letterheadImageFormUpdate.ImageBase64;//這段之後會做成IMAGE64的處理並另存在指定的位置

                    letterheadImageJournal.ImagePath = imagePath;
                    BaseInputLetterheadImageJournal(letterheadImageJournal, false, userId);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.UpdateAccountantSignNoData();
                    response.ErrorItem = "Update updateLetterheadImageId: " + letterheadImageFormUpdate.Id;
                    responseViewModels.Add(response);
                }
            }
            //新增信頭圖像
            foreach (LetterheadImageForm letterheadImageForm in letterheadImageUpdate.CreateLetterheadImages)
            {
                //這段之後會做成IMAGE64的處理並另存在指定的位置
                string imagePath = letterheadImageForm.ImageBase64;
                LetterheadImageCheck letterheadImageCheck = new()
                {
                    LetterheadId = letterheadImageForm.LetterheadId,
                    SealMappingConfigId = letterheadImageForm.SealMappingConfigId,
                    GroupCreateDate = letterheadImageUpdate.GroupCreateDate,
                    Sequence = letterheadImageForm.Sequence
                };

                if (!CheckLetterheadImageRepeat(letterheadImageCheck, letterheadImageUpdate.DeleteLetterheadImageIds))
                {
                    LetterheadImageJournal letterheadImageJournal = mapper.Map<LetterheadImageJournal>(letterheadImageForm);
                    letterheadImageJournal.ImagePath = imagePath;
                    letterheadImageJournal.GroupCreateDate = letterheadImageUpdate.GroupCreateDate;
                    BaseInputLetterheadImageJournal(letterheadImageJournal, true, userId);
                    letterheadImageJournals.Add(letterheadImageJournal);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.CreateLetterheadImageSequenceRepeat();
                    response.ErrorItem = "Create LetterheadId:" + letterheadImageForm.LetterheadId
                                       + " SealMappingConfigId:" + letterheadImageForm.SealMappingConfigId;
                    responseViewModels.Add(response);
                }
            }
            //沒有任何回傳訊息(錯誤訊息)就更新資料庫
            if (!responseViewModels.Any())
            {
                ResponseViewModel response = new();

                //將此創建日期的圖片審查狀態全變更為草稿(更新時需要重審)
                IQueryable<LetterheadImageJournal> letterheadImageJournalQuery = dbContext.LetterheadImageJournals.Where
                                                (
                                                    letterheadImage => letterheadImage.LetterheadId == letterheadImageUpdate.LetterheadId
                                                    && letterheadImage.GroupCreateDate == letterheadImageUpdate.GroupCreateDate
                                                    && letterheadImage.DeleteStatus == DeleteStatus.NO
                                                );
                foreach (LetterheadImageJournal letterheadImageJournal in letterheadImageJournalQuery)
                {
                    letterheadImageJournal.ReviewStatus = ReviewStatus.Draft;
                }

                dbContext.LetterheadImageJournals.AddRange(letterheadImageJournals);
                dbContext.BulkSaveChanges();
                response.Success();
                responseViewModels.Add(response);
            }
            return responseViewModels;
        }

        /// <summary>
        /// 變更此群組創建日期的信頭圖片審核為通過(啟用)
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch">會計師簽印群組創建日期</param>        
        /// <returns></returns>
        public ResponseViewModel UpdateReviewStatusApprovalLetterheadImages(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch)
        {
            ResponseViewModel response = ChangeDraftReviewStatusLetterheadImage(letterheadImageGroupCreateDateSearch, ReviewStatus.Approval);
            return response;
        }

        /// <summary>
        /// 變更此群組群組創建日期的信頭圖片審核為作廢
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch">會計師簽印群組創建日期</param>        
        public ResponseViewModel UpdateReviewStatusInvalidLetterheadImages(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch)
        {
            ResponseViewModel response = ChangeDraftReviewStatusLetterheadImage(letterheadImageGroupCreateDateSearch, ReviewStatus.Invalid);
            return response;
        }

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="letterheadImageJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">建立或更新此檔的user的Id</param>
        private static void BaseInputLetterheadImageJournal(LetterheadImageJournal letterheadImageJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                letterheadImageJournal.CreateUserId = userId;
                letterheadImageJournal.CreateDate = DateTime.Now;
                letterheadImageJournal.DeleteStatus = DeleteStatus.NO;
            }
            else
            {
                letterheadImageJournal.UpdateUserId = userId;
                letterheadImageJournal.UpdateDate = DateTime.Now;
            }
            letterheadImageJournal.StartDate = AvailableDateUtil.NotActivated();
            letterheadImageJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            letterheadImageJournal.ReviewStatus = ReviewStatus.Draft; //建立或是更新簽印都會變成草稿狀態
        }

        /// <summary>
        /// 確認此類別會計師簽印是否重覆建立 true 重複 false 不重複
        /// </summary>
        /// <param name="ltterheadImageCheck">查詢參數</param>
        /// <param name="DeleteLetterheadImageIds"></param>
        /// <returns></returns>
        private bool CheckLetterheadImageRepeat(LetterheadImageCheck ltterheadImageCheck, List<int> DeleteLetterheadImageIds)
        {
            LetterheadImageJournal? LetterheadImageQuery = dbContext.LetterheadImageJournals
                                                        .FirstOrDefault
                                                        (
                                                            letterheadImage => letterheadImage.LetterheadId == ltterheadImageCheck.LetterheadId
                                                            && letterheadImage.SealMappingConfigId == ltterheadImageCheck.SealMappingConfigId
                                                            && letterheadImage.GroupCreateDate == ltterheadImageCheck.GroupCreateDate
                                                            && letterheadImage.Sequence == ltterheadImageCheck.Sequence
                                                            && letterheadImage.DeleteStatus == DeleteStatus.NO
                                                            && letterheadImage.ReviewStatus <= ReviewStatus.Pending
                                                            && !DeleteLetterheadImageIds.Contains(letterheadImage.Id)
                                                        );
            return LetterheadImageQuery != null;
        }

        /// <summary>
        /// 信頭草稿狀態變更。
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch">客戶ID與季度</param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeDraftReviewStatusLetterheadImage(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userId = 0;//從帳號驗證取得
            IQueryable<LetterheadImageJournal> letterheadImageJournalQuery = dbContext.LetterheadImageJournals.Where
                                                            (
                                                                letterheadImage => letterheadImage.LetterheadId == letterheadImageGroupCreateDateSearch.LetterheadId
                                                                && letterheadImage.GroupCreateDate == letterheadImageGroupCreateDateSearch.GroupCreateDate
                                                                && letterheadImage.ReviewStatus == ReviewStatus.Draft
                                                            );

            if (letterheadImageJournalQuery.Any())
            {
                foreach (LetterheadImageJournal letterheadImageJournal in letterheadImageJournalQuery)
                {
                    letterheadImageJournal.ReviewStatus = reviewStatus;
                    letterheadImageJournal.UpdateDate = DateTime.Now;
                    letterheadImageJournal.UpdateUserId = userId;
                }
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.UpdateLetterheadImageNoData();
            }
            return response;
        }
    }
}
