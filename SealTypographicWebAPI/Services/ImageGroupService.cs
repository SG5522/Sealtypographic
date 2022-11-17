using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.DbModels;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Customer;
using EFCore.BulkExtensions;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
    /// </summary>
    public class ImageGroupService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ResponseService responseService;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="responseService"></param>        
        public ImageGroupService(SealTypographicDbContext dbContext, ResponseService responseService)
        {
            this.dbContext = dbContext;
            this.responseService = responseService;
        }

        /// <summary>
        /// 取得圖片群組
        /// </summary>
        /// <param name="imageGroupId">圖片群組ID</param>
        /// <returns></returns>
        public ImageGroupResponse GetImageGroup(int imageGroupId)
        {
            ImageGroupViewModel imageGroupViewModel = new();
            Response response = new();
            var imageGroupQuery = dbContext.ImageGroups.Where(imageGroup => imageGroup.Id == imageGroupId);
            if(imageGroupQuery.Any())
            {
                ImageGroup imageGroup = imageGroupQuery.First();
                imageGroupViewModel.Id = imageGroup.Id;
                imageGroupViewModel.Name = imageGroup.Name;
                imageGroupViewModel.Type = imageGroup.Type;
                imageGroupViewModel.Description = imageGroup.Description;
                
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }
            return new ImageGroupResponse()
            {
                ImageGroup = imageGroupViewModel,

                Code = response.Code,
                Message= response.Message
            };
        }

        /// <summary>
        /// 取得圖片群組資料列
        /// </summary>
        /// <param name="imageGroupQuery"></param>
        /// <returns></returns>
        public ImageGroupResponsePage GetimageGroupResponsePage(ImageGroupQuery imageGroupQuery)
        {
            List<ImageGroupViewModel> imageGroupViewModels = new();
            Response response = new();
            int totalPage = 0;
            int totalCount = 0;
            var imageGroupQuerys = dbContext.ImageGroups.AsQueryable();
            if (imageGroupQuery.NameOrType != null)
            {
                imageGroupQuerys = imageGroupQuerys.Where
                (
                    imageGroup =>
                    imageGroup.Name.Contains(imageGroupQuery.NameOrType)
                    || imageGroup.Type.Contains(imageGroupQuery.NameOrType)
                );
            }

            imageGroupQuerys = imageGroupQuerys.OrderBy(imageGroup => imageGroup.Id);

            if (imageGroupQuerys.Any())
            {
                //取得該頁            
                var pageNumberImageGroups = imageGroupQuerys
                                            .Skip((imageGroupQuery.PageNumber - 1) * imageGroupQuery.PageSize)
                                            .Take(imageGroupQuery.PageSize)
                                            .ToList();
                //計算總頁數
                totalPage = (imageGroupQuerys.Count() / imageGroupQuery.PageSize) + (imageGroupQuerys.Count() % imageGroupQuery.PageSize == 0 ? 0 : 1);
                totalCount = imageGroupQuerys.Count();
                foreach (var imageGroup in pageNumberImageGroups)
                {
                    imageGroupViewModels.Add(new ()
                    {
                        Id = imageGroup.Id,
                        Name = imageGroup.Name,
                        Type = imageGroup.Type,
                        Description = imageGroup.Description                        
                    });
                }
                //取得成功訊息
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }
            return new ImageGroupResponsePage() 
            { 
                PageNumber = imageGroupQuery.PageNumber,
                TotalPage = totalPage,
                TotalCount= totalCount,
                ImageGroup = imageGroupViewModels,

                Code = response.Code,
                Message = response.Message
            };
        }

        /// <summary>
        /// 建立圖片群組
        /// </summary>
        /// <param name="imageGroupViewModel">圖片群組資料</param>
        /// <returns></returns>
        public Response CreateImageGroup (ImageGroupViewModel imageGroupViewModel)
        {
            Response response = new();
            var imageGroupQuery = dbContext.ImageGroups
                                .Where(imageGroup => imageGroup.Id == imageGroup.Id);

            if (!imageGroupQuery.Any())
            {
                ImageGroup imageGroup = new()
                {
                    Id = imageGroupViewModel.Id,
                    Name = imageGroupViewModel.Name,
                    Type = imageGroupViewModel.Type, 
                    Description = imageGroupViewModel.Description
                };
                dbContext.ImageGroups.Add(imageGroup);
                dbContext.SaveChanges();
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.UniqueConstraintFailed);
            }

            return response;
        }
        /// <summary>
        /// 更新圖片群組資料
        /// </summary>
        /// <param name="imageGroupViewModel">圖片群組資料 imageGroupViewModel.id 為搜尋條件</param>        
        public Response UpdateImageGroup(ImageGroupViewModel imageGroupViewModel)
        {
            Response response = new();
            var imageGroupQuery = dbContext.ImageGroups
                                .Where(imageGroup => imageGroup.Id == imageGroupViewModel.Id);

            if (imageGroupQuery.Any())
            {
                var imageGroup = imageGroupQuery.First();
                imageGroup.Name = imageGroupViewModel.Name;
                imageGroup.Type = imageGroupViewModel.Type;
                imageGroup.Description = imageGroupViewModel.Description;
                dbContext.SaveChanges();
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }
            return response;
        }       
    }
}
