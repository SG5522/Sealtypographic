using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Utils;
using Serilog;
using System.Linq;

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
            List<TemporarySealViewModel> logViewModel = new();
            TemporarySealQuarterJournal? temporarySealGroup = dbContext.TemporarySealQuarterJournals.Include
                                                                                    (
                                                                                        temporarySealGroup => temporarySealGroup.TemporarySealJournals.Where
                                                                                        (x => x.DeleteStatus == DeleteStatus.No)
                                                                                    )
                                                                                  .Include(temporarySealGroup => temporarySealGroup.Customer)
                                                                                  .FirstOrDefault(temporarySealGroup => temporarySealGroup.Id == temporaryId);                                                                                  

            if(temporarySealGroup != null)
            {
                temporarySealDetailViewModel.CustomerId = temporarySealGroup.Customer.Id;
                temporarySealDetailViewModel.CustomerName = temporarySealGroup.Customer.Name;
                

                foreach (TemporarySealJournal temporarySealJournal in temporarySealGroup.TemporarySealJournals)
                {
                    TemporarySealViewModel temporarySealViewModel = new()
                    {
                        Id = temporarySealJournal.Id,
                        Sequence = temporarySealJournal.Sequence,
                        ImageBase64 = imageSharpService.GetPathToBase64(temporarySealJournal.ImageFullPath)                        
                    };                    
                    temporarySealDetailViewModel.ViewModels.Add(temporarySealViewModel);                    
                }                
                temporarySealDetailViewModel.Success();

                logViewModel = temporarySealDetailViewModel.ViewModels.Select(x => { x.ImageBase64 = string.Empty; return x;}).ToList();
                Log.Information("TemporarySeal detail output {@Output}", logViewModel);
            }
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
            
            int companyId = 1;

            IQueryable<TemporarySealQuarterJournal> temporarySealGroupQuery = dbContext.TemporarySealQuarterJournals
                                                                    .Where
                                                                    (
                                                                        temporarySealGroup => temporarySealGroup.Customer.Company.Id == companyId
                                                                        && temporarySealGroup.Customer.DeleteStatus == DeleteStatus.No
                                                                        && temporarySealGroup.DeleteStatus == DeleteStatus.No
                                                                    );

            if (!string.IsNullOrEmpty(temporarySealSearch.KeyWord))
            {
                temporarySealGroupQuery = temporarySealGroupQuery
                                            .Where
                                            (
                                                temporarySealGroup => temporarySealGroup.Quarter.Contains(temporarySealSearch.KeyWord)
                                                || temporarySealGroup.Customer.Name.Contains(temporarySealSearch.KeyWord)
                                            );
            }
            temporarySealGroupQuery = temporarySealGroupQuery.OrderBy(temporarySealGroup => temporarySealGroup.Id);

            if(temporarySealGroupQuery.Any())
            {

                List<TemporaryViewModel> thisPageTemporarySealGroups = temporarySealGroupQuery
                                                                      .Include(temporarySealGroup => temporarySealGroup.Customer)
                                                                      .Skip((temporarySealSearch.PageNumber - 1) * temporarySealSearch.PageSize)
                                                                      .Take(temporarySealSearch.PageSize)
                                                                      .Select(temporarySealGroup => new TemporaryViewModel()
                                                                      {
                                                                          Id = temporarySealGroup.Id,
                                                                          CustomerName = temporarySealGroup.Customer.Name,
                                                                          Quarter = temporarySealGroup.Quarter,
                                                                      })
                                                                      .ToList();

                temporarySealPaginateViewModel.ViewModels = thisPageTemporarySealGroups;
                temporarySealPaginateViewModel.PageNumber = temporarySealSearch.PageNumber;
                temporarySealPaginateViewModel.PageSize = temporarySealSearch.PageSize;
                //計算總頁數
                temporarySealPaginateViewModel.TotalPage = TotalPageUtil.GetTotalPage(temporarySealGroupQuery.Count(), temporarySealSearch.PageSize);
                temporarySealPaginateViewModel.TotalCount = temporarySealGroupQuery.Count();
            }
            temporarySealPaginateViewModel.Success();

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
                                    .Include(customer => customer.TemporarySealGroups)                                          
                                    .FirstOrDefault(customer => customer.Id == temporarySealForm.CustomerId);
            if (customerQuery != null) 
            {
                TemporarySealQuarterJournal temporarySealGroup = new();
                List<TemporarySealJournal> temporarySealJournals = new();
                int userId = 0;
                ImageBase64Info imageBase64Info = new()
                {
                    Code = customerQuery.Code,
                    SealType = SealType.TemporarySeal
                };

                temporarySealGroup.Quarter = temporarySealForm.Quarter;                
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
        /// <param name="temporarySealUpdateForm">基本資料</param>
        public ResponseViewModel Update(int Id, TemporarySealUpdateForm temporarySealUpdateForm)
        {
            ResponseViewModel response = new();
            int userId = 0;
            TemporarySealQuarterJournal? temporarySealGroupQuery = dbContext.TemporarySealQuarterJournals.Include(temporarySealGroup => temporarySealGroup.TemporarySealJournals)
                                                                                       .FirstOrDefault(temporarySealGroup => temporarySealGroup.Id == Id);
            if(temporarySealGroupQuery != null)
            {
                ImageBase64Info imageBase64Info = new()
                {
                    Code = temporarySealGroupQuery.Customer.Code,
                    SealType = SealType.TemporarySeal
                };

                temporarySealGroupQuery.Customer.Id = temporarySealUpdateForm.CustomerId;
                temporarySealGroupQuery.Quarter = temporarySealUpdateForm.Quarter;
                BaseInputTemporarySealGroup(temporarySealGroupQuery, false, userId);

                //更新臨時章印鑑組
                foreach (TemporarySealUpdate temporarySealUpdate in temporarySealUpdateForm.SealsToUpdate)
                {
                    TemporarySealJournal? temporarySealJournalQuery = temporarySealGroupQuery.TemporarySealJournals.FirstOrDefault(x => x.Id == temporarySealUpdate.Id);
                    if(temporarySealJournalQuery != null)
                    {
                        imageBase64Info.ImageBase64 = temporarySealUpdate.ImageBase64;
                        //新增更新後的臨時章
                        TemporarySealJournal temporarySealJournal = new()
                        {
                            Sequence = temporarySealUpdate.Sequence,
                            ImageFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, false),
                            ThumbnailFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, true)
                        };                        
                        BaseInputTemporarySealJournal(temporarySealJournal, true, userId);
                        temporarySealGroupQuery.TemporarySealJournals.Add(temporarySealJournal);

                        //將更新的ID帶入刪除List
                        temporarySealUpdateForm.SealIdsToDelete.Add(temporarySealUpdate.Id);

                        //將原始臨時章標上刪除
                        temporarySealJournalQuery.DeleteStatus = DeleteStatus.Yes;
                        BaseInputTemporarySealJournal(temporarySealJournalQuery, false, userId);
                    }
                    else
                    {
                        response.ErrorItem += $"Update temporarySeal NoData:{temporarySealUpdate.Id}";
                    }
                }

                //找出需要刪除&更新的臨時章
                IQueryable<TemporarySealJournal> deleteSeals = temporarySealGroupQuery.TemporarySealJournals
                                                                                                .Where(x => !temporarySealUpdateForm.SealIdsToDelete.Contains(x.Id))
                                                                                                .AsQueryable();
                //標記為刪除
                foreach(TemporarySealJournal deleteSeal in deleteSeals)
                {
                    deleteSeal.DeleteStatus = DeleteStatus.Yes;
                    BaseInputTemporarySealJournal(deleteSeal, false, userId);
                }

                //新增臨時章
                foreach (TemporarySeal temporarySeal in temporarySealUpdateForm.SealsToCreate)
                {
                    imageBase64Info.ImageBase64 = temporarySeal.ImageBase64;
                    //新增臨時章
                    TemporarySealJournal temporarySealJournal = new()
                    {
                        Sequence = temporarySeal.Sequence,
                        ImageFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, false),
                        ThumbnailFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, true)
                    };
                    BaseInputTemporarySealJournal(temporarySealJournal, true, userId);
                    temporarySealGroupQuery.TemporarySealJournals.Add(temporarySealJournal);
                }

                if (response.ErrorItem == null)
                {
                    dbContext.SaveChanges();
                    response.Success();
                }                
                else
                {                    
                    response.UpdateTemporarySealNoData();
                }
            }
            else
            {
                response.UpdateTemporarySealNoData();
            }

            return response;
        }

        /// <summary>
        /// 刪除臨時章。
        /// </summary>
        /// <param name="Id"></param>
        public ResponseViewModel Delete(int Id)
        {
            ResponseViewModel response = new();
            int userId = 0;
            TemporarySealQuarterJournal? temporarySealGroup = dbContext.TemporarySealQuarterJournals.Find(Id);
            if(temporarySealGroup != null)
            {
                temporarySealGroup.DeleteStatus = DeleteStatus.Yes;
                BaseInputTemporarySealGroup(temporarySealGroup, false, userId);
                response.Success();
            }
            else
            {
                response.DeleteTemporarySealNoData();
            }
            return response;
        }



        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="temporarySealGroup">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputTemporarySealGroup(TemporarySealQuarterJournal temporarySealGroup, bool isCreate, int userId)
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
