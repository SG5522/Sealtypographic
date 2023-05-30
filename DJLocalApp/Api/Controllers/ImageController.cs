using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SealAPIWrap;
using SealAPIWrap.Models;

namespace DJLocalApp.Api.Controllers
{
    /// <summary>
    /// 影像功能
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ISealService sealService;
        private readonly IMapper mapper;

        /// <summary>
        /// 建構：注入Service
        /// </summary>
        public ImageController(ISealService sealService, IMapper mapper)
        {
            this.sealService = sealService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 建立印鑑
        /// </summary>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public SealBuildResult SealBuild(SealBuildRequest request)
        {
            SealBuildForm form = mapper.Map<SealBuildForm>(request);
            form.Config = GetSealApiConfig();
            return sealService.Operation<SealBuildForm, SealBuildResult>(OPMode.SealBuild, form);
        }

        /// <summary>
        /// 驗印
        /// </summary>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public SealIdentifyResult SealIdentify(SealIdentifyRequest request)
        {
            SealIdentifyForm form = mapper.Map<SealIdentifyForm>(request);
            form.Config = GetSealApiConfig();
            return sealService.Operation<SealIdentifyForm, SealIdentifyResult>(OPMode.SealIdentify, form);
        }

        /// <summary>
        /// 驗印結果輔助顯示
        /// </summary>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public SealShowResult SealShow(SealShowRequest request)
        {
            SealShowForm form = mapper.Map<SealShowForm>(request);
            form.Config = GetSealApiConfig();
            return sealService.Operation<SealShowForm, SealShowResult>(OPMode.SealShow, form);
        }

        /// <summary>
        /// 圖像處理
        /// </summary>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public ImageProcessResult ImageProcess(ImageProcessRequest request)
        {
            ImageProcessForm form = mapper.Map<ImageProcessForm>(request);
            form.Config = GetSealApiConfig();
            return sealService.Operation<ImageProcessForm, ImageProcessResult>(OPMode.ImageProcess, form);
        }

        private string GetSealApiConfig()
        {
            return AppDomain.CurrentDomain.BaseDirectory + @"Resources\lib\config";
        }
    }
}
