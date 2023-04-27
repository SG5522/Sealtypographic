using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Utils;
using Serilog;

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
        private readonly SealPathOption sealPathOption;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="imageService"></param>
        /// <param name="mapper"></param>
        /// <param name="options"></param>
        public TemporarySealService(SealTypographicDbContext dbContext, ImageService imageService, IMapper mapper, IOptionsSnapshot<SealPathOption> options)
        {
            this.dbContext = dbContext;
            this.imageSharpService = imageService;
            this.mapper = mapper;
            this.sealPathOption = options.Value;
        }

        /// <summary>
        /// 取得臨時章詳細基本資料
        /// </summary>
        /// <param name="temporaryId">臨時章ID</param>
        /// <returns></returns>
        public TemporarySealDetailViewModel GetDetail(int temporaryId)
        {
            TemporarySealDetailViewModel temporarySealDetailViewModel = new();
            TemporarySealDetailLogModel logModel = new();

            TemporarySealDetailViewModel? temporarySealGroup = dbContext.TemporarySealQuarterJournals.AsNoTracking()
                                                                .Include(temporarySealGroup => temporarySealGroup.TemporarySealJournals)
                                                                .Include(temporarySealGroup => temporarySealGroup.Customer)
                                                                .Where(temporarySealGroup => temporarySealGroup.Id == temporaryId)                                                                                    
                                                                .Select(
                                                                    x => new TemporarySealDetailViewModel()
                                                                    {
                                                                        CustomerId = x.Customer.Id,
                                                                        CustomerName = x.Customer.Name,
                                                                        ViewModels = x.TemporarySealJournals
                                                                        .Where(x => x.DeleteStatus == DeleteStatus.No)
                                                                        .Select(x => new TemporarySealViewModel() 
                                                                        { 
                                                                            Id = x.Id,
                                                                            Sequence = x.Sequence,
                                                                            ImageFullPath = x.ImageFullPath
                                                                        }).ToList()
                                                                    }
                                                                ).FirstOrDefault();

            if(temporarySealGroup != null)
            {           
                logModel = mapper.Map<TemporarySealDetailLogModel>(temporarySealDetailViewModel);
                foreach (TemporarySealViewModel temporarySealViewModel in temporarySealGroup.ViewModels)
                {
                    //取得路徑轉換ImageBase64
                    temporarySealViewModel.ImageBase64 = imageSharpService.GetPathToBase64(temporarySealViewModel.ImageFullPath);
                    //LOG紀錄用
                    TemporarySealLogModel temporarySealLogModel = mapper.Map<TemporarySealLogModel>(temporarySealViewModel);                    
                    temporarySealLogModel.ImageFileName = Path.GetFileName(temporarySealViewModel.ImageFullPath);
                    logModel.ViewModels.Add(temporarySealLogModel);
                }
                temporarySealDetailViewModel = temporarySealGroup;
                temporarySealDetailViewModel.Success();                
                Log.Information("TemporarySeal detail output {@Output}", logModel);
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
                                                temporarySealGroup => temporarySealGroup.Customer.Name.Contains(temporarySealSearch.KeyWord)
                                                //|| temporarySealGroup.Quarter.Contains(temporarySealSearch.KeyWord)
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
                                    .Include(customer => customer.TemporarySealQuarterJournals)                                    
                                    .Select(customer => new Customer
                                    {
                                        Id = customer.Id,
                                        Code = customer.Code,
                                        TemporarySealQuarterJournals = new List<TemporarySealQuarterJournal>()
                                    })
                                    .FirstOrDefault(customer => customer.Id == temporarySealForm.CustomerId);
            
            if (customerQuery != null) 
            {
                TemporarySealQuarterJournal temporarySealQuarterJournal = new();
                List<TemporarySealJournal> temporarySealJournals = new();
                ImageBase64Info imageBase64Info = SetImageBase64Info(customerQuery.Code);
                int userId = 0;
                
                temporarySealQuarterJournal.Quarter = temporarySealForm.Quarter;                
                BaseInputTemporarySealGroup(temporarySealQuarterJournal, true, userId);
                
                foreach (TemporarySeal temporarySeal in temporarySealForm.Seals)
                {
                    TemporarySealJournal temporarySealJournal = new()
                    {
                        Sequence = temporarySeal.Sequence
                    };
                    imageBase64Info.ImageBase64 = temporarySeal.ImageBase64;
                    temporarySealJournal.ImageFullPath = imageSharpService.GetSavedImageFilePath(imageBase64Info, false);
                    temporarySealJournal.ThumbnailFullPath = imageSharpService.GetSavedImageFilePath(imageBase64Info, true);

                    BaseInputTemporarySealJournal(temporarySealJournal, true, userId);
                    temporarySealJournals.Add(temporarySealJournal);
                }

                temporarySealQuarterJournal.TemporarySealJournals = temporarySealJournals;
                customerQuery.TemporarySealQuarterJournals.Add(temporarySealQuarterJournal);
                dbContext.Entry(customerQuery).State = EntityState.Unchanged;
                dbContext.TemporarySealQuarterJournals.Add(temporarySealQuarterJournal);                              
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
        /// <param name="temporarySealUpdateForm">基本資料</param>
        public ResponseViewModel Update(TemporarySealUpdateForm temporarySealUpdateForm)
        {
            ResponseViewModel response = new();
            int userId = 0;
            TemporarySealQuarterJournal? temporarySealQuarterJournalQuery = dbContext.TemporarySealQuarterJournals
                                                                    .Include(temporarySealQuarterJournal => temporarySealQuarterJournal.Customer)
                                                                    .Include(temporarySealQuarterJournal => temporarySealQuarterJournal.TemporarySealJournals)
                                                                    .Select 
                                                                    (
                                                                        temporarySealQuarterJournal => new TemporarySealQuarterJournal
                                                                        {
                                                                            Id = temporarySealUpdateForm.Id,
                                                                            Customer = new Customer{ 
                                                                                Id = temporarySealQuarterJournal.Customer.Id,
                                                                                Code = temporarySealQuarterJournal.Customer.Code,
                                                                            },
                                                                            TemporarySealJournals = temporarySealQuarterJournal.TemporarySealJournals
                                                                                                    .Where(x => x.DeleteStatus == DeleteStatus.No).ToList()
                                                                        }
                                                                    )
                                                                    .FirstOrDefault(temporarySealGroup => temporarySealGroup.Id == temporarySealUpdateForm.Id);
            if(temporarySealQuarterJournalQuery != null)
            {
                ImageBase64Info imageBase64Info = SetImageBase64Info(temporarySealQuarterJournalQuery.Customer.Code);

                //更新臨時章印鑑組
                foreach (TemporarySealUpdate temporarySealUpdate in temporarySealUpdateForm.SealsToUpdate)
                {
                    TemporarySealJournal? temporarySealJournalQuery = temporarySealQuarterJournalQuery.TemporarySealJournals.FirstOrDefault(x => x.Id == temporarySealUpdate.Id);
                    if(temporarySealJournalQuery != null)
                    {
                        imageBase64Info.ImageBase64 = temporarySealUpdate.ImageBase64;
                        //新增更新後的臨時章
                        TemporarySealJournal temporarySealJournal = new()
                        {
                            Sequence = temporarySealUpdate.Sequence,
                            ImageFullPath = imageSharpService.GetSavedImageFilePath(imageBase64Info, false),
                            ThumbnailFullPath = imageSharpService.GetSavedImageFilePath(imageBase64Info, true)
                        };                        
                        BaseInputTemporarySealJournal(temporarySealJournal, true, userId);
                        temporarySealQuarterJournalQuery.TemporarySealJournals.Add(temporarySealJournal);

                        //將更新的ID帶入刪除List
                        temporarySealUpdateForm.SealIdsToDelete.Add(temporarySealUpdate.Id);
                    }
                    else
                    {
                        response.ErrorItem += $"Update temporarySeal NoData Id:{temporarySealUpdate.Id}";
                    }
                }

                //找出需要刪除&更新的臨時章
                IQueryable<TemporarySealJournal> deleteSeals = temporarySealQuarterJournalQuery.TemporarySealJournals
                                                                                        .Where(x => temporarySealUpdateForm.SealIdsToDelete.Contains(x.Id))
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
                        ImageFullPath = imageSharpService.GetSavedImageFilePath(imageBase64Info, false),
                        ThumbnailFullPath = imageSharpService.GetSavedImageFilePath(imageBase64Info, true)
                    };
                    BaseInputTemporarySealJournal(temporarySealJournal, true, userId);
                    temporarySealQuarterJournalQuery.TemporarySealJournals.Add(temporarySealJournal);
                }

                if (response.ErrorItem == null)
                {                    
                    dbContext.Attach(temporarySealQuarterJournalQuery);
                    dbContext.SaveChanges();
                    response.Success();
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


        /// <summary>
        /// 設定ImageBase64Info
        /// </summary>
        /// <param name="code">編碼(檔名結構之一)</param>
        /// <returns></returns>
        private ImageBase64Info SetImageBase64Info(string code)
        {
            ImageBase64Info imageBase64Info = new()
            {
                Code = code,
                SealType = SealType.TemporarySeal,
                SaveRootPath = sealPathOption.TemporarySeal
            };
            return imageBase64Info;
        }
    }
}
