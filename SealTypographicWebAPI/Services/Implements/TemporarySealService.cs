using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Temporary;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 管理臨時章資料
    /// </summary>
    public class TemporarySealService : ITemporarySealService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageSharpService;
        private readonly IMapper mapper;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="imageService"></param>
        /// <param name="mapper"></param>
        public TemporarySealService(SealTypographicDbContext dbContext, ImageService imageService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.imageSharpService = imageService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得臨時章詳細基本資料
        /// </summary>
        /// <param name="temporaryId">臨時章ID</param>
        /// <returns></returns>
        public TemporarySealDetailViewModel GetDetail(int temporaryId)
        {
            TemporarySealDetailViewModel temporarySealDetailViewModel = new();
            return temporarySealDetailViewModel;
        }

        /// <summary>
        /// 取得臨時章資料列表(分頁)
        /// </summary>
        /// <param name="temporarySealSearch">臨時章分頁搜尋</param>        
        /// <returns></returns>
        public TemporarySealPaginateViewModel GetPaginate(TemporarySealSearch temporarySealSearch)
        {
            TemporarySealPaginateViewModel temporarySealPaginateViewModel = new();
            return temporarySealPaginateViewModel;
        }

        /// <summary>
        /// 新增客戶基本資料
        /// </summary>
        /// <param name="temporarySealForm">基本資料</param>
        public ResponseViewModel New(TemporarySealForm temporarySealForm)
        {
            ResponseViewModel response = new ();                                    
            
            Customer? customerQuery = dbContext.Customers
                                          .Include(x => x.TemporarySealGroups)
                                          .ThenInclude(x => x.TemporarySealJournals)
                                          .FirstOrDefault(x => x.Id == temporarySealForm.CustomerId);
            if (customerQuery != null) 
            {
                TemporarySealGroup temporarySealGroup = new();
                List<TemporarySealJournal> temporarySealJournals = new();
                int userId = 0;
                ImageBase64Info imageBase64Info = new()
                {
                    Code = customerQuery.Code,
                    SealType = SealType.TemporarySeal
                };

                temporarySealGroup.Name = temporarySealForm.Name;                
                BaseInputTemporarySealGroup(temporarySealGroup, true, userId);
                

                foreach (TemporarySeal temporarySeal in temporarySealForm.Seals)
                {
                    TemporarySealJournal temporarySealJournal = new()
                    {
                        Sequence = temporarySeal.Sequence
                    };
                    imageBase64Info.ImageBase64 = temporarySeal.ImageBase64;
                    temporarySealJournal.ImageFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, false);
                    temporarySealJournal.ThumbnailFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, true);
                    BaseInputTemporarySealJournal(temporarySealJournal, true, userId);
                    temporarySealJournals.Add(temporarySealJournal);
                }
                temporarySealGroup.TemporarySealJournals = temporarySealJournals;
                customerQuery.TemporarySealGroups.Add(temporarySealGroup);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.CustomeNoData();
            }
            return response;
        }

        /// <summary>
        /// 更新臨時章
        /// </summary>
        /// <param name="Id">臨時章Id</param>
        /// <param name="temporarySealForm">基本資料</param>
        public ResponseViewModel Update(int Id, TemporarySealForm temporarySealForm)
        {
            ResponseViewModel responseViewModel = new();
            return responseViewModel;
        }

        /// <summary>
        /// 刪除臨時章。
        /// </summary>
        /// <param name="Id"></param>
        public ResponseViewModel Delete(int Id)
        {
            ResponseViewModel responseViewModel = new();
            return responseViewModel;
        }


        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="temporarySealGroup">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputTemporarySealGroup(TemporarySealGroup temporarySealGroup, bool isCreate, int userId)
        {
            if (isCreate)
            {
                temporarySealGroup.CreateUserId = userId;
                temporarySealGroup.CreateDate = DateTime.Now;
                temporarySealGroup.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                temporarySealGroup.UpdateUserId = userId;
                temporarySealGroup.UpdateDate = DateTime.Now;
            }
        }


        /// <summary>
        /// 臨時章新增修改時基本的資料輸入
        /// </summary>
        /// <param name="temporarySealJournal">Db上的臨時章資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputTemporarySealJournal(TemporarySealJournal temporarySealJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                temporarySealJournal.CreateUserId = userId;
                temporarySealJournal.CreateDate = DateTime.Now;
                temporarySealJournal.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                temporarySealJournal.UpdateUserId = userId;
                temporarySealJournal.UpdateDate = DateTime.Now;
            }
        }
    }
}
