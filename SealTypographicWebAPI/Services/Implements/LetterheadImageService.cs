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
using System.Linq;

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
        /// 取得信頭名稱與圖片建立日期
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        public LetterheadImageCreateDateViews GetNameAndCreateDate(int letterheadId)
        {
            LetterheadImageCreateDateViews letterheadImageCreateDateViews = new();

            Letterhead? letterhead = dbContext.Letterheads.Include(x => x.LetterheadImageJournals)
                                    .FirstOrDefault(letterhead => letterhead.Id == letterheadId);

            if (letterhead != null)
            {
                letterheadImageCreateDateViews.Name = letterhead.Name;
                letterheadImageCreateDateViews.CreateDateViews = letterhead.LetterheadImageJournals
                                                                .Where(x => x.DeleteStatus == DeleteStatus.No)
                                                                .Select(letterheadImageJournal => new LetterheadImageCreateDateView()
                                                                {
                                                                    Id = letterheadImageJournal.Id,
                                                                    GroupCreateDate = letterheadImageJournal.CreateDate,
                                                                    Status = EnumExtenstionUtil.GetDescription(letterheadImageJournal.Status)
                                                                })
                                                                .OrderByDescending(x => x.Id)
                                                                .ToList();
                letterheadImageCreateDateViews.Success();
            }
            else
            {
                letterheadImageCreateDateViews.LetterheadImageNoData();
            }
            return letterheadImageCreateDateViews;
        }

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="id">信頭圖片Id</param>
        /// <returns></returns>
        public LetterheadImageViewModel GetImageViewModel(int id)
        {
            LetterheadImageViewModel letterheadImageViewModels = new()
            {                
                Id = id,
            };

            LetterheadImageJournal? letterheadImageJournalQuery = dbContext.LetterheadImageJournals.FirstOrDefault
                                                                    (
                                                                        x => x.Id == id
                                                                        && x.DeleteStatus == DeleteStatus.No                    
                                                                    );      
                
            if (letterheadImageJournalQuery != null)
            {                                
                letterheadImageViewModels.ImageBase64 = imageSharpService.GetPathToBase64(letterheadImageJournalQuery.ImageFullPath); //資料庫取得圖檔路徑轉BASE64                                                                                  
                letterheadImageViewModels.Success();
            }
            else
            {
                letterheadImageViewModels.LetterheadImageNoData();
            }

            return letterheadImageViewModels;            
        }

        /// <summary>
        /// 新增信頭圖片
        /// </summary>
        /// <param name="letterheadImageForms">信頭圖片</param>
        /// <returns></returns>
        public ResponseViewModel New(LetterheadImageForm letterheadImageForms)
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
        /// 異動信頭圖片
        /// </summary>
        /// <param name="letterheadImageUpdate">異動信頭圖片資料</param>
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
                response.ErrorItem = $"Update updateLetterheadImageId:{letterheadImageUpdate.Id}";
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
                letterheadImageJournal.DeleteStatus = DeleteStatus.No;
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
                letterhead.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                letterhead.UpdateUserId = userid;
                letterhead.UpdateDate = DateTime.Now;
            }
        }
    }
}
