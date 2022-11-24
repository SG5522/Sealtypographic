using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Customer;
using EFCore.BulkExtensions;
using SealTypographicWebAPI.Util;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
    /// </summary>
    public class SealMappingConfigService
    {
        private readonly SealTypographicDbContext dbContext;        

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>          
        public SealMappingConfigService(SealTypographicDbContext dbContext)
        {
            this.dbContext = dbContext;            
        }

        /// <summary>
        /// 取得圖片群組
        /// </summary>
        /// <param name="imageGroupId">圖片群組ID</param>
        /// <returns></returns>
        public SealMappingConfigResponse GetImageGroup(int imageGroupId)
        {
            SealMappingConfigViewModel sealMappingConfigViewModel = new();
            Response response = new();
            IQueryable<SealMappingConfig> sealMappingConfigQuery = dbContext.SealMappingConfigs.Where(imageGroup => imageGroup.Id == imageGroupId);
            if(sealMappingConfigQuery.Any())
            {
                SealMappingConfig sealMappingConfig = sealMappingConfigQuery.First();                
                sealMappingConfigViewModel.Name = sealMappingConfig.Name;
                sealMappingConfigViewModel.Type = sealMappingConfig.Type;
                sealMappingConfigViewModel.SubId = sealMappingConfig.SubId;
                
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }
            return new SealMappingConfigResponse()
            {
                ImageGroup = sealMappingConfigViewModel,

                Code = response.Code,
                Message= response.Message
            };
        }

        /// <summary>
        /// 取得圖片群組資料列
        /// </summary>
        /// <param name="SealMappingConfigQuery"></param>
        /// <returns></returns>
        public SealMappingConfigResponsePage GetSealMappingConfigResponsePage(SealMappingConfigQuery SealMappingConfigQuery)
        {
            List<SealMappingConfigViewModel> sealMappingConfigViewModels = new();
            Response response = new();
            int totalPage = 0;
            int totalCount = 0;
            IQueryable<SealMappingConfig> sealMappingConfigQuerys = dbContext.SealMappingConfigs.AsQueryable();
            if (SealMappingConfigQuery.NameOrType != null)
            {
                sealMappingConfigQuerys = sealMappingConfigQuerys.Where
                (
                    sealMappingConfig =>
                    sealMappingConfig.Name.Contains(SealMappingConfigQuery.NameOrType)
                    || sealMappingConfig.Type.Contains(SealMappingConfigQuery.NameOrType)
                );
            }

            sealMappingConfigQuerys = sealMappingConfigQuerys.OrderBy(sealMappingConfig => sealMappingConfig.Id);

            if (sealMappingConfigQuerys.Any())
            {
                //取得該頁            
                var pageNumberSealMappingConfigs = sealMappingConfigQuerys
                                            .Skip((SealMappingConfigQuery.PageNumber - 1) * SealMappingConfigQuery.PageSize)
                                            .Take(SealMappingConfigQuery.PageSize)
                                            .ToList();
                //計算總頁數
                totalPage = (sealMappingConfigQuerys.Count() / SealMappingConfigQuery.PageSize) + (sealMappingConfigQuerys.Count() % SealMappingConfigQuery.PageSize == 0 ? 0 : 1);
                totalCount = sealMappingConfigQuerys.Count();
                foreach (var sealMappingConfig in pageNumberSealMappingConfigs)
                {
                    sealMappingConfigViewModels.Add(new ()
                    {                        
                        Name = sealMappingConfig.Name,
                        Type = sealMappingConfig.Type,
                        SubId = sealMappingConfig.SubId                        
                    });
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }
            return new SealMappingConfigResponsePage() 
            { 
                PageNumber = SealMappingConfigQuery.PageNumber,
                TotalPage = totalPage,
                TotalCount= totalCount,
                ImageGroup = sealMappingConfigViewModels,

                Code = response.Code,
                Message = response.Message
            };
        }

        /// <summary>
        /// 建立圖片群組
        /// </summary>
        /// <param name="sealMappingConfigViewModel">圖片群組資料</param>
        /// <returns></returns>
        public Response CreateImageGroup (SealMappingConfigViewModel sealMappingConfigViewModel)
        {
            Response response = new();
            IQueryable<SealMappingConfig> sealMappingConfigQuery = dbContext.SealMappingConfigs
                                .Where(sealMappingConfig => sealMappingConfig.SubId == sealMappingConfigViewModel.SubId);

            if (!sealMappingConfigQuery.Any())
            {
                SealMappingConfig imageGroup = new()
                {                                        
                    Type = sealMappingConfigViewModel.Type,
                    SubId = sealMappingConfigViewModel.SubId,
                    Name = sealMappingConfigViewModel.Name
                };
                dbContext.SealMappingConfigs.Add(imageGroup);
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.UniqueConstraintFailed();
            }

            return response;
        }
        /// <summary>
        /// 更新圖片群組資料
        /// </summary>
        /// <param name="SealMappingConfigViewModel">圖片群組資料 imageGroupViewModel.id 為搜尋條件</param>        
        public Response UpdateImageGroup(SealMappingConfigViewModel SealMappingConfigViewModel)
        {
            Response response = new();
            IQueryable<SealMappingConfig> SealMappingConfigQuery = dbContext.SealMappingConfigs
                                .Where(imageGroup => imageGroup.SubId == SealMappingConfigViewModel.SubId);

            if (SealMappingConfigQuery.Any())
            {
                SealMappingConfig SealMappingConfig = SealMappingConfigQuery.First();                
                SealMappingConfig.Type = SealMappingConfigViewModel.Type;
                SealMappingConfig.SubId = SealMappingConfigViewModel.SubId;
                SealMappingConfig.Name = SealMappingConfigViewModel.Name;                                
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }
            return response;
        }       
    }
}
