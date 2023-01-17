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
        private readonly ImageService imageSharpService;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageSharpService"></param>
        public LetterheadImageService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageSharpService)
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
        public LetterheadGroupCreateDateViews GetLetterheadCreateDate(int letterheadId)
        {
            LetterheadGroupCreateDateViews letterheadGroupCreateDateViews = new();
            List<LetterheadImageGroupCreateDateView> groupCreateDateViews = dbContext.LetterheadImageCreateDateJournal
                                                                            .Include(x => x.Letterhead)
                                                                           .Where
                                                                           (
                                                                                letterheadImageCreateJournal => letterheadImageCreateJournal.Letterhead.Id == letterheadId                                                                                
                                                                                && letterheadImageCreateJournal.DeleteStatus == DeleteStatus.NO
                                                                           )
                                                                           .Select(letterheadImageCreateJournal => new LetterheadImageGroupCreateDateView()
                                                                           {
                                                                               LetterheadId = letterheadId,
                                                                               LetterheadImageCreateId = letterheadImageCreateJournal.Id,
                                                                               GroupCreateDate = letterheadImageCreateJournal.CreateDate,
                                                                               ReviewStatus = letterheadImageCreateJournal.ReviewStatus
                                                                           })
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
        public LetterheadImageViewModels GetImage (LetterheadImageGroupCreateDateSearch letterheadGroupCreateDateSearch)
        {
            LetterheadImageViewModels letterheadImageViewModels = new()
            {
                LetterheadId = letterheadGroupCreateDateSearch.LetterheadId,
                GroupCreateDate = letterheadGroupCreateDateSearch.GroupCreateDate
            };

            LetterheadImageCreateDateJournal? letterheadImageCreateJournalQuery = dbContext.LetterheadImageCreateDateJournal.Find(letterheadGroupCreateDateSearch.LetterheadImageCreateId);      
                
            if (letterheadImageCreateJournalQuery != null)
            {
                foreach (LetterheadImageJournal letterheadImageJournal in letterheadImageCreateJournalQuery.LetterheadImageJournals)
                {
                    LetterheadImageViewModel letterheadImageViewModel = mapper.Map<LetterheadImageViewModel>(letterheadImageJournal);
                    letterheadImageViewModel.ImageBase64 = imageSharpService.GetPathToBase64(letterheadImageJournal.ImagePath, SealType.Letterhead); //資料庫取得圖檔路徑轉BASE64                                                      
                    letterheadImageViewModels.ImageViewModels.Add(letterheadImageViewModel);
                    //ImageViewModels.Add(letterheadImageViewModel);
                }
                letterheadImageViewModels.ReviewStatus = letterheadImageCreateJournalQuery.ReviewStatus;                
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
            //List<LetterheadImageJournal> letterheadImageJournals = new();
            int userId = 0; //以後從帳號驗證取得Id
            int count = 1;
            ImageBase64Info imageBase64Info = new()
            {
                Code = GetCode(letterheadImageForms.LetterheadId),
                SealType = SealType.Customer
            };
            DateTime createNowTime = DateTime.Now;//將建立日期為依據將此次建立的會計師簽印組成為一組Group
            LetterheadImageCreateDateJournal letterheadImageCreateJournal = new()
            {
                CreateDate = createNowTime
            };

            BaseInputCreateDateJournal(letterheadImageCreateJournal, true, userId);

            foreach (LetterheadImageForm letterheadImage in letterheadImageForms.ImageForms)
            {
                LetterheadImageJournal letterheadImageJournal = new();


                //ImageBase64轉圖檔並存到指定資料夾
                imageBase64Info.ImageBase64 = letterheadImage.ImageBase64;
                letterheadImageJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    

                BaseInputImageJournal(letterheadImageJournal, true, userId);
                
                letterheadImageCreateJournal.LetterheadImageJournals.Add(letterheadImageJournal);                
                count++;                                      
            }
            dbContext.LetterheadImageCreateDateJournal.Add(letterheadImageCreateJournal);
            dbContext.SaveChanges();
            response.Success();
            
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
            LetterheadImageCreateDateJournal? createDateJournalQuery = dbContext.LetterheadImageCreateDateJournal.Find(letterheadImageUpdate.LetterheadImageCreateDateId);

            if(createDateJournalQuery != null)
            {                
                //修改信頭圖像
                foreach (LetterheadImageFormUpdate letterheadImageFormUpdate in letterheadImageUpdate.UpdateLetterheadImages)
                {
                    LetterheadImageJournal? updateImageQuery = dbContext.LetterheadImageJournals.Find(letterheadImageFormUpdate.Id);

                    if (updateImageQuery != null)
                    {
                        LetterheadImageJournal letterheadImageJournal = new();

                        //ImageBase64轉圖檔並存到指定資料夾                    
                        imageBase64Info.ImageBase64 = letterheadImageFormUpdate.ImageBase64;
                        letterheadImageJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);

                        BaseInputImageJournal(letterheadImageJournal, true, userId);

                        createDateJournalQuery.LetterheadImageJournals.Add(letterheadImageJournal);
                        //dbContext.SealReviewJournals.Add(sealReviewJournal);

                        //原圖片刪除(Hide)
                        updateImageQuery.DeleteStatus = DeleteStatus.Yes;                        
                        BaseInputImageJournal(updateImageQuery, false, userId);                        

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
                    LetterheadImageJournal letterheadImageJournal = new();


                    //ImageBase64轉圖檔並存到指定資料夾
                    imageBase64Info.ImageBase64 = createLetterheadImage.ImageBase64;
                    letterheadImageJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);
                    BaseInputImageJournal(letterheadImageJournal, true, userId);
                    createDateJournalQuery.LetterheadImageJournals.Add(letterheadImageJournal);
                    count++;                    
                }

                //沒有任何回傳訊息(錯誤訊息)就更新資料庫
                if (!responseViewModels.Any())
                {
                    ResponseViewModel response = new();

                    //將此創建日期的圖片審查狀態全變更為草稿(更新時需要重審)
                    createDateJournalQuery.ReviewStatus = ReviewStatus.Draft;

                    dbContext.SaveChanges();
                    response.Success();
                    responseViewModels.Add(response);
                }
            }
            
            return responseViewModels;
        }
        

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="letterheadImageJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">建立或更新此檔的user的Id</param>
        private static void BaseInputImageJournal(LetterheadImageJournal letterheadImageJournal, bool isCreate, int userId)
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
        /// <param name="letterheadImageCreateDateJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputCreateDateJournal(LetterheadImageCreateDateJournal letterheadImageCreateDateJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                letterheadImageCreateDateJournal.CreateUserId = userId;                
                letterheadImageCreateDateJournal.DeleteStatus = DeleteStatus.NO;
            }
            else
            {
                letterheadImageCreateDateJournal.UpdateUserId = userId;
                letterheadImageCreateDateJournal.UpdateDate = DateTime.Now;
            }
            letterheadImageCreateDateJournal.StartDate = AvailableDateUtil.NotActivated();
            letterheadImageCreateDateJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            letterheadImageCreateDateJournal.ReviewStatus = ReviewStatus.Draft;
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
