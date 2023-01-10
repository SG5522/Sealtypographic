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
        private readonly ImageSharpService imageSharpService;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageSharpService"></param>
        public LetterheadImageService(SealTypographicDbContext dbContext, IMapper mapper, ImageSharpService imageSharpService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageSharpService = imageSharpService;
        }

        /// <summary>
        /// 取得信頭圖片群組創建日期列表
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        public LetterheadGroupCreateDateViews GetLetterheadCreateDateViews(int letterheadId)
        {
            LetterheadGroupCreateDateViews letterheadGroupCreateDateViews = new();
            List<LetterheadImageGroupCreateDateView> groupCreateDateViews = dbContext.SealReviewJournals
                                                                            .Include(x => x.LetterheadImageJournal)
                                                                           .Where
                                                                           (
                                                                                sealReviewJournal => sealReviewJournal.LetterheadImageJournal.LetterheadId == letterheadId                                                                                
                                                                                && sealReviewJournal.ReviewStatus <= ReviewStatus.Draft //草稿狀態以下顯示
                                                                                && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                                                           )
                                                                           .Select(sealReviewJournal => new LetterheadImageGroupCreateDateView()
                                                                           {
                                                                               LetterheadId = letterheadId,
                                                                               GroupCreateDate = sealReviewJournal.CreateDate,
                                                                               ReviewStatus = sealReviewJournal.ReviewStatus
                                                                           })
                                                                           .GroupBy(LetterheadImageGroupCreateDateView => LetterheadImageGroupCreateDateView.GroupCreateDate)
                                                                           .OrderByDescending(g => g.Key)
                                                                           .Select(LetterheadImageGroupCreateDateView => LetterheadImageGroupCreateDateView.First())
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
            List<SealReviewJournal> sealReviewJournalQuery = dbContext.SealReviewJournals.Where
                                                                    (
                                                                        sealReviewJournal => 
                                                                        sealReviewJournal.LetterheadImageJournal.LetterheadId == letterheadGroupCreateDateSearch.LetterheadId
                                                                        && sealReviewJournal.CreateDate == letterheadGroupCreateDateSearch.GroupCreateDate
                                                                        && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                                                        && sealReviewJournal.ReviewStatus <= ReviewStatus.Draft
                                                                    )                         
                                                                    .Include(sealReviewJournal => sealReviewJournal.LetterheadImageJournal)
                                                                    .OrderBy(sealReviewJournal => sealReviewJournal.Sequence)
                                                                    .ToList();
            if (sealReviewJournalQuery.Any())
            {
                foreach (SealReviewJournal sealReviewJournal in sealReviewJournalQuery)
                {
                    LetterheadImageViewModel letterheadImageViewModel = mapper.Map<LetterheadImageViewModel>(sealReviewJournal);
                    letterheadImageViewModel.ImageBase64 = imageSharpService.GetPathToBase64(sealReviewJournal.LetterheadImageJournal.ImagePath, SealType.Letterhead); //資料庫取得圖檔路徑轉BASE64                   
                                   
                    ImageViewModels.Add(letterheadImageViewModel);
                }
                letterheadImageViewModels.ReviewStatus = sealReviewJournalQuery.First().ReviewStatus;
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
        public ResponseViewModel Create(LetterheadImageForms letterheadImageForms)
        {
            ResponseViewModel response = new();
            List<LetterheadImageJournal> letterheadImageJournals = new();
            int userId = 0; //以後從帳號驗證取得Id

            SealReviewJournal? sealReviewJournalQuery = dbContext.SealReviewJournals.FirstOrDefault
                                                            (
                                                                x => x.LetterheadImageJournal.LetterheadId == letterheadImageForms.LetterheadId                                                                                                
                                                                && x.ReviewStatus >= ReviewStatus.Draft
                                                                && x.ReviewStatus <= ReviewStatus.Pending
                                                            );
            if (sealReviewJournalQuery == null)
            {
                int count = 1;
                ImageBase64Info imageBase64Info = new()
                {
                    Code = GetCode(letterheadImageForms.LetterheadId),
                    SealType = SealType.Customer
                };

                DateTime createNowTime = DateTime.Now;//將建立日期為依據將此次建立的會計師簽印組成為一組Group
                foreach (LetterheadImageForm letterheadImage in letterheadImageForms.ImageForms)
                {
                    LetterheadImageJournal letterheadImageJournal = new()
                    {
                        LetterheadId = letterheadImageForms.LetterheadId
                    };
                    SealReviewJournal sealReviewJournal = new()
                    {                        
                        Sequence = letterheadImage.Sequence,
                        CreateDate = createNowTime
                    };

                    //ImageBase64轉圖檔並存到指定資料夾
                    imageBase64Info.ImageBase64 = letterheadImage.ImageBase64;
                    letterheadImageJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    

                    BaseInputLetterheadImageJournal(letterheadImageJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.LetterheadImageJournal = letterheadImageJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);
                    count++;                                      
                }

                dbContext.SaveChanges();
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
        public List<ResponseViewModel> Update(LetterheadImageUpdate letterheadImageUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();            
            int userId = 0;//之後會從帳號驗證中取得userid
            int count = 1;
            ImageBase64Info imageBase64Info = new()
            {
                Code = GetCode(letterheadImageUpdate.LetterheadId),
                SealType = SealType.Letterhead,                
            };

            //刪除印鑑
            foreach (int sealReview in letterheadImageUpdate.DeleteLetterheadImageIds)
            {
                SealReviewJournal? deleteSealQuery = dbContext.SealReviewJournals
                                                                .Include(sealReview => sealReview.LetterheadImageJournal)
                                                                .FirstOrDefault
                                                                (
                                                                    letterheadImage => letterheadImage.Id == sealReview
                                                                    && letterheadImage.DeleteStatus == DeleteStatus.NO
                                                                    && letterheadImage.ReviewStatus <= ReviewStatus.Draft
                                                                );
                if (deleteSealQuery != null)
                {
                    deleteSealQuery.DeleteStatus = DeleteStatus.Yes;
                    deleteSealQuery.LetterheadImageJournal.DeleteStatus = DeleteStatus.Yes;
                    BaseInputLetterheadImageJournal(deleteSealQuery.LetterheadImageJournal, false, userId);
                    BaseInputSealReviewJournal(deleteSealQuery, false, userId);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.DeleteAccountantSignNoData();
                    response.ErrorItem = "Delete letterheadImageJournalId:" + sealReview;
                    responseViewModels.Add(response);
                }
            }
            //修改信頭圖像
            foreach (LetterheadImageFormUpdate letterheadImageFormUpdate in letterheadImageUpdate.UpdateLetterheadImages)
            {
                SealReviewJournal? updateSealQuery = dbContext.SealReviewJournals
                                                            .Include(sealReview => sealReview.LetterheadImageJournal)
                                                            .FirstOrDefault
                                                            (
                                                                sealReview => sealReview.Id == letterheadImageFormUpdate.Id
                                                                && sealReview.DeleteStatus == DeleteStatus.NO
                                                                && sealReview.ReviewStatus <= ReviewStatus.Draft
                                                            );
                if (updateSealQuery != null)
                {
                    //新增印鑑                    
                    SealReviewJournal sealReviewJournal = new()
                    {                        
                        Sequence = letterheadImageFormUpdate.Sequence,
                        CreateDate = letterheadImageUpdate.GroupCreateDate
                    };
                    LetterheadImageJournal letterheadImageJournal = new()
                    {
                        LetterheadId = letterheadImageUpdate.LetterheadId
                    };
                    //ImageBase64轉圖檔並存到指定資料夾                    
                    imageBase64Info.ImageBase64 = letterheadImageFormUpdate.ImageBase64;                    
                    letterheadImageJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    

                    BaseInputLetterheadImageJournal(letterheadImageJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.LetterheadImageJournal = letterheadImageJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);

                    //原圖片刪除(Hide)
                    updateSealQuery.DeleteStatus = DeleteStatus.Yes;
                    updateSealQuery.LetterheadImageJournal.DeleteStatus = DeleteStatus.Yes;
                    BaseInputLetterheadImageJournal(updateSealQuery.LetterheadImageJournal, false, userId);
                    BaseInputSealReviewJournal(updateSealQuery, false, userId);

                    count++;
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
            foreach (LetterheadImageForm createLetterheadImage in letterheadImageUpdate.CreateLetterheadImages)
            {
                //這段之後會做成IMAGE64的處理並另存在指定的位置
                string imagePath = createLetterheadImage.ImageBase64;
                LetterheadImageCheck letterheadImageCheck = new()
                {
                    LetterheadId = letterheadImageUpdate.LetterheadId,                    
                    GroupCreateDate = letterheadImageUpdate.GroupCreateDate,
                    Sequence = createLetterheadImage.Sequence
                };

                List<int> updateLetterheadImageIds = letterheadImageUpdate.UpdateLetterheadImages.Select(x => x.Id).ToList();

                if (!CheckLetterheadImageRepeat(letterheadImageCheck, letterheadImageUpdate.DeleteLetterheadImageIds, updateLetterheadImageIds))
                {
                    //新增印鑑                    
                    LetterheadImageJournal letterheadImageJournal = new()
                    {
                        LetterheadId = letterheadImageUpdate.LetterheadId
                    };                    
                    SealReviewJournal sealReviewJournal = new()
                    {
                        Sequence = letterheadImageCheck.Sequence,
                        CreateDate = letterheadImageUpdate.GroupCreateDate
                    };

                    imageBase64Info.ImageBase64 = createLetterheadImage.ImageBase64;
                    letterheadImageJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);
                    
                    BaseInputLetterheadImageJournal(letterheadImageJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.LetterheadImageJournal = letterheadImageJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);

                    count++;
                }
                else
                {
                    ResponseViewModel response = new();
                    response.CreateLetterheadImageSequenceRepeat();
                    response.ErrorItem = "Create LetterheadId:" + letterheadImageUpdate.LetterheadId
                                       + " Sequence:" + createLetterheadImage.Sequence;
                    responseViewModels.Add(response);
                }
            }
            //沒有任何回傳訊息(錯誤訊息)就更新資料庫
            if (!responseViewModels.Any())
            {
                ResponseViewModel response = new();

                //將此創建日期的圖片審查狀態全變更為草稿(更新時需要重審)
                IQueryable<SealReviewJournal> sealReviewJournalQuery = dbContext.SealReviewJournals.Where
                                                (
                                                    sealReview => sealReview.LetterheadImageJournal.LetterheadId == letterheadImageUpdate.LetterheadId
                                                    && sealReview.CreateDate == letterheadImageUpdate.GroupCreateDate
                                                    && sealReview.DeleteStatus == DeleteStatus.NO
                                                );
                foreach (SealReviewJournal sealReviewJournal in sealReviewJournalQuery)
                {
                    sealReviewJournal.ReviewStatus = ReviewStatus.Draft;
                }
                
                dbContext.SaveChanges();
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
        public ResponseViewModel ApprovalLetterheadImages(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch)
        {
            ResponseViewModel response = ChangeDraftReviewStatus(letterheadImageGroupCreateDateSearch, ReviewStatus.Approval);
            return response;
        }

        /// <summary>
        /// 變更此群組群組創建日期的信頭圖片審核為作廢
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch">會計師簽印群組創建日期</param>        
        public ResponseViewModel InvalidLetterheadImages(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch)
        {
            ResponseViewModel response = ChangeDraftReviewStatus(letterheadImageGroupCreateDateSearch, ReviewStatus.Invalid);
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
        }

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="sealReviewJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputSealReviewJournal(SealReviewJournal sealReviewJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                sealReviewJournal.CreateUserId = userId;                
                sealReviewJournal.DeleteStatus = DeleteStatus.NO;
            }
            else
            {
                sealReviewJournal.UpdateUserId = userId;
                sealReviewJournal.UpdateDate = DateTime.Now;
            }
            sealReviewJournal.StartDate = AvailableDateUtil.NotActivated();
            sealReviewJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            sealReviewJournal.ReviewStatus = ReviewStatus.Draft;
        }

        /// <summary>
        /// 確認此類別會計師簽印是否重覆建立 true 重複 false 不重複
        /// </summary>
        /// <param name="ltterheadImageCheck">查詢參數</param>
        /// <param name="deleteLetterheadImageIds">異動中刪除的圖片Id</param>
        /// <param name="updateLetterheadImageIds">異動中更新的圖片Id(也會被標上刪除)</param>
        /// <returns></returns>
        private bool CheckLetterheadImageRepeat(LetterheadImageCheck ltterheadImageCheck, List<int> deleteLetterheadImageIds, List<int> updateLetterheadImageIds)
        {
            SealReviewJournal? sealReviewQuery = dbContext.SealReviewJournals
                                                        .FirstOrDefault
                                                        (
                                                            sealReviewJournal => sealReviewJournal.LetterheadImageJournal.Letterhead.Id == ltterheadImageCheck.LetterheadId                                                            
                                                            && sealReviewJournal.CreateDate == ltterheadImageCheck.GroupCreateDate
                                                            && sealReviewJournal.Sequence == ltterheadImageCheck.Sequence
                                                            && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                                            && sealReviewJournal.ReviewStatus <= ReviewStatus.Pending
                                                            && !deleteLetterheadImageIds.Contains(sealReviewJournal.Id)
                                                            && !updateLetterheadImageIds.Contains(sealReviewJournal.Id)
                                                        );
            return sealReviewQuery != null;
        }

        /// <summary>
        /// 信頭草稿狀態變更。
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch">客戶ID與季度</param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeDraftReviewStatus(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userId = 0;//從帳號驗證取得
            IQueryable<SealReviewJournal> sealReviewJournalQuery = dbContext.SealReviewJournals.Where
                                                            (
                                                                sealReviewJournal => 
                                                                sealReviewJournal.LetterheadImageJournal.LetterheadId == letterheadImageGroupCreateDateSearch.LetterheadId
                                                                && sealReviewJournal.CreateDate == letterheadImageGroupCreateDateSearch.GroupCreateDate
                                                                && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                                                && sealReviewJournal.ReviewStatus == ReviewStatus.Draft
                                                            );

            if (sealReviewJournalQuery.Any())
            {
                foreach (SealReviewJournal sealReview in sealReviewJournalQuery)
                {
                    sealReview.ReviewStatus = reviewStatus;
                    sealReview.UpdateDate = DateTime.Now;
                    sealReview.UpdateUserId = userId;
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

        private string GetCode(int letterheadId)
        {
            string code;
            Letterhead? letterhead = dbContext.Letterheads.Find(letterheadId);
            if (letterhead != null)
            {
                code = letterhead.Code;
            }
            else
            {
                code = string.Empty;
            }
            return code;
        }
    }
}
