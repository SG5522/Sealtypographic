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
        public LetterheadCreateDateViews GetCreateDate(int letterheadId)
        {
            LetterheadCreateDateViews letterheadGroupCreateDateViews = new();
            List<LetterheadImageCreateDateView> groupCreateDateViews = dbContext.LetterheadImageJournals
                                                                            .Include(x => x.Letterhead)
                                                                           .Where
                                                                           (
                                                                                letterheadImageCreateJournal => letterheadImageCreateJournal.Letterhead.Id == letterheadId
                                                                                && letterheadImageCreateJournal.DeleteStatus == DeleteStatus.NO
                                                                           )
                                                                           .Select(letterheadImageJournal => new LetterheadImageCreateDateView()
                                                                           {
                                                                               LetterheadImageId = letterheadImageJournal.Id,
                                                                               GroupCreateDate = letterheadImageJournal.CreateDate,
                                                                               Status = EnumExtenstionUtil.GetDescription(letterheadImageJournal.Status)
                                                                           })
                                                                           .OrderBy(x => x.LetterheadImageId)
                                                                           .ToList();

            if (groupCreateDateViews.Any())
            {
                letterheadGroupCreateDateViews.LetterheadId = letterheadId;
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
        /// <param name="letterheadImageSearch">搜尋條件</param>
        /// <returns></returns>
        public LetterheadImageViewModel GetImage (LetterheadImageSearch letterheadImageSearch)
        {
            LetterheadImageViewModel letterheadImageViewModels = new()
            {                
                LetterheadImageId = letterheadImageSearch.LetterheadImageId,
            };

            LetterheadImageJournal? letterheadImageJournalQuery = dbContext.LetterheadImageJournals.Find(letterheadImageSearch.LetterheadImageId);      
                
            if (letterheadImageJournalQuery != null)
            {                                
                letterheadImageViewModels.ImageBase64 = imageSharpService.GetPathToBase64(letterheadImageJournalQuery.ImageFullPath, SealType.Letterhead); //資料庫取得圖檔路徑轉BASE64                                                                                  
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
            //DateTime createNowTime = DateTime.Now;//建立日期            
            Letterhead letterhead = new();
            LetterheadImageJournal letterheadImage = new();
            List<LetterheadImageJournal> letterheadImages = new();
            int userId = 0; //以後從帳號驗證取得Id
            int count = 1;
            ImageBase64Info imageBase64Info = new() 
            {                                 
                SealType = SealType.Letterhead 
            };

            //信頭基本資料
            letterhead.Name = letterheadImageForms.Name;            
            BaseInputLetterhead(letterhead, true, userId);
            //ImageBase64轉圖檔並存到指定資料夾
            imageBase64Info.ImageBase64 = letterheadImageForms.ImageBase64;
            letterheadImage.ImageFullPath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);
            //新增信頭圖片
            BaseInputImageJournal(letterheadImage, true, userId);
            letterheadImages.Add(letterheadImage);
            //關連信頭基本資料
            letterhead.LetterheadImageJournals = letterheadImages;
            
            dbContext.Letterheads.Add(letterhead);
            dbContext.SaveChanges();
            response.Success();
            
            return response;
        }

        /// <summary>
        /// 異動會計師簽印的處理(審查狀態退回或是草稿才進行修改)
        /// </summary>
        /// <param name="letterheadImageUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        public ResponseViewModel Update(LetterheadImageUpdate letterheadImageUpdate)
        {
            ResponseViewModel response = new();            
            int userId = 0;//之後會從帳號驗證中取得userid
            int count = 1;
            ImageBase64Info imageBase64Info = new()
            {                
                SealType = SealType.Letterhead,                
            };
            LetterheadImageJournal? updateImageQuery = dbContext.LetterheadImageJournals
                                                        .Include(letterheadImageJournal => letterheadImageJournal.Letterhead)
                                                        .FirstOrDefault(letterheadImageJournal => letterheadImageJournal.Id == letterheadImageUpdate.Id);
            if (updateImageQuery != null)
            {
                LetterheadImageJournal letterheadImageJournal = new();

                //ImageBase64轉圖檔並存到指定資料夾                    
                imageBase64Info.ImageBase64 = letterheadImageUpdate.ImageBase64;
                letterheadImageJournal.ImageFullPath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);

                BaseInputImageJournal(letterheadImageJournal, true, userId);
                letterheadImageJournal.Letterhead = updateImageQuery.Letterhead;

                dbContext.LetterheadImageJournals.Add(letterheadImageJournal);

                updateImageQuery.Letterhead.Name = letterheadImageUpdate.LetterheadName;
                BaseInputLetterhead(updateImageQuery.Letterhead, false, userId);

                //原圖片狀態變更停用                
                BaseInputImageJournal(updateImageQuery, false, userId);

                dbContext.SaveChanges();
                response.Success();                
            }
            else
            {                
                response.UpdateAccountantSignNoData();
                response.ErrorItem = "Update updateLetterheadImageId: " + letterheadImageUpdate.Id;                
            }
            
            return response;
        }
        

        /// <summary>
        /// 信頭圖片新增修改時基本的資料輸入
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
                letterheadImageJournal.Status = LetterheadImageStatus.Enable;
            }
            else
            {
                letterheadImageJournal.UpdateUserId = userId;
                letterheadImageJournal.UpdateDate = DateTime.Now;
                letterheadImageJournal.Status = LetterheadImageStatus.Disabled;
            }  
            
        }
        private static void BaseInputLetterhead(Letterhead letterhead, bool isCreate, int userid)
        {
            if (isCreate)
            {
                letterhead.CreateUserId = userid;
                letterhead.CreateDate = DateTime.Now;
                letterhead.DeleteStatus = DeleteStatus.NO;
            }
            else
            {
                letterhead.UpdateUserId = userid;
                letterhead.UpdateDate = DateTime.Now;
            }
        }
    }
}
