using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Customer;
using EFCore.BulkExtensions;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Models.SealMappingConfig;
using AutoMapper;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
    /// </summary>
    public class SealMappingConfigService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>          
        public SealMappingConfigService(SealTypographicDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得圖片群組
        /// </summary>
        /// <param name="imageGroupId">圖片群組ID</param>
        /// <returns></returns>
        public SealMappingConfigResponse GetImageGroup(int imageGroupId)
        {
            SealMappingConfigViewModel sealMappingConfigViewModel = new();
            ResponseViewModel response = new();
            IQueryable<SealMappingConfig> sealMappingConfigQuery = dbContext.SealMappingConfigs.Where(imageGroup => imageGroup.Id == imageGroupId);
            if(sealMappingConfigQuery.Any())
            {
                SealMappingConfig sealMappingConfig = sealMappingConfigQuery.First();                
                sealMappingConfigViewModel.Name = sealMappingConfig.Name;
                sealMappingConfigViewModel.SealType = sealMappingConfig.SealType;
                sealMappingConfigViewModel.SubId = sealMappingConfig.SubId;
                
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
            }
            return new SealMappingConfigResponse()
            {
                SealMappingConfigViewModel = sealMappingConfigViewModel,

                Code = response.Code,
                Message= response.Message
            };
        }

        /// <summary>
        /// 取得印鑑群組(客戶)資料列
        /// </summary>
        /// <returns></returns>
        public SealMappingConfigResponseList GetSealMappingConfigResponseList(SealType sealType)
        {
            List<SealMappingConfigViewModel> sealMappingConfigViewModels = new();
            ResponseViewModel response = new();
            List<SealMappingConfig> sealMappingConfigQuerys = dbContext.SealMappingConfigs.Where(sealMappingConfig => sealMappingConfig.SealType == sealType).ToList();
            if (sealMappingConfigQuerys.Any())
            {
                foreach (SealMappingConfig sealMappingConfig in sealMappingConfigQuerys)
                {
                    sealMappingConfigViewModels.Add(mapper.Map<SealMappingConfigViewModel>(sealMappingConfig));
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
            }
            return new SealMappingConfigResponseList() 
            { 
                Code = response.Code,
                Message = response.Message,

                SealMappingConfigViewModel = sealMappingConfigViewModels,
            };
        }

        /// <summary>
        /// 建立圖片群組
        /// </summary>
        /// <param name="sealMappingConfigViewModel">圖片群組資料</param>
        /// <returns></returns>
        public ResponseViewModel CreateSealMappingConfig (SealMappingConfigViewModel sealMappingConfigViewModel)
        {
            ResponseViewModel response = new();
            IQueryable<SealMappingConfig> sealMappingConfigQuery = dbContext.SealMappingConfigs
                                .Where(sealMappingConfig => sealMappingConfig.SubId == sealMappingConfigViewModel.SubId);

            if (!sealMappingConfigQuery.Any())
            {
                SealMappingConfig sealMappingConfig = mapper.Map<SealMappingConfig>(sealMappingConfigViewModel);
                dbContext.SealMappingConfigs.Add(sealMappingConfig);
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
        /// <param name="sealMappingConfigViewModel">圖片群組資料 sealMappingConfigViewModel.id 為搜尋條件</param>        
        public ResponseViewModel UpdateSealMappingConfig (SealMappingConfigViewModel sealMappingConfigViewModel)
        {
            ResponseViewModel response = new();
            SealMappingConfig? SealMappingConfigQuery = dbContext.SealMappingConfigs
                                .Where(sealMappingConfig => sealMappingConfig.Id == sealMappingConfigViewModel.Id)
                                .FirstOrDefault();

            if (SealMappingConfigQuery != null)
            {                
                mapper.Map(sealMappingConfigViewModel, SealMappingConfigQuery);
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
            }
            return response;
        }       
    }
}
