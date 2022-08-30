using Microsoft.AspNetCore.Mvc;
using SealTypographic.Models;
using TchznSeal;
using Newtonsoft.Json;
using SealTypographic.Service;

namespace SealTypographic.Controllers
{
    public class CreateSealsSetController : Controller
    {
        private readonly string ScanImagePath = @".\wwwroot\Sealcard\";
        private readonly string SealTempPath = Path.GetTempPath() + @"\Seal\";     

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 印鑑建檔頁面 讀取資料夾所有圖檔
        /// </summary>
        /// <returns></returns>
        public IActionResult SelectSealCard()
        {
            List<ImageData> imageDatas = GetImageList(ScanImagePath);
            return View(imageDatas);
        }

        /// <summary>
        /// 客戶章頁面
        /// </summary>
        /// <param name="imageName">被選擇的掃描圖檔名</param>
        /// <returns></returns>
        public IActionResult CustomerSealset(string imageName)
        {
            ViewBag.imageName = imageName;
            return View();
        }

        /// <summary>
        /// 會計章頁面
        /// </summary>
        /// <param name="imageName">被選擇的掃描圖檔名</param>
        /// <returns></returns>
        public IActionResult AccountingSealset(string imageName)
        {
            List<Seal> seals = new();
            AutoSealSplit autoSealSplit = new();
            int count = 1;
            //@呼叫天創元件進行分離 每次會自己產生bmp檔 後續需要請天創調整元件
            autoSealSplit.SplitSeal(ScanImagePath + imageName , SealTempPath, "Test", "R");
            List<ImageData> sealImage = GetImageList(SealTempPath);
            foreach (ImageData imageData in sealImage)
            {                
                seals.Add(new Seal
                {
                    SealImageName = imageData.FileName,
                    SealName = "test" + count.ToString(),
                    SetNo = count.ToString(),
                    SealNo = "1",
                });
                count++;
            }            
            
            ViewBag.imageName = imageName;
            return View(seals);
        }

        private SealSet SealSetSetting()
        {
            SealSet sealSet = new();
            return sealSet;
        }

        /// <summary>
        /// 信頭紙頁面
        /// </summary>
        /// <param name="imageName">被選擇的掃描圖檔名</param>
        /// <returns></returns>
        public IActionResult LetterHeadSealset(string imageName)
        {
            ViewBag.imageName = imageName;
            return View();
        }

        /// <summary>
        /// 獲得資料夾內的所有圖檔以及圖檔的檔名
        /// </summary>
        /// <param name="imagePath">圖檔路徑</param>        
        /// <returns></returns>
        private static List<ImageData> GetImageList(string imagePath)
        {
            List<ImageData> images = new();
            string[] files = Directory.GetFiles(imagePath);
            if (files.Length > 0)
            {
                DirectoryInfo folder = new(imagePath);
                //指定讀取資料夾內的圖檔type
                foreach (FileInfo file in folder.GetFiles("*.*"))
                {                                        
                    images.Add(new ImageData
                    {
                        FileName = file.Name,
                        FileFullName = file.FullName,                        
                        ContentType = ImageGetData.ImageType(file.Name),                        
                    });
                }
            }
            return images;
        }        
    }
}