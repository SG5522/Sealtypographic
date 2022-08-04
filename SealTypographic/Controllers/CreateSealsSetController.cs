using Microsoft.AspNetCore.Mvc;
using DJLib;
using SealTypographic.Models;
using TchznSeal;
using Newtonsoft.Json;

namespace SealTypographic.Controllers
{
    public class CreateSealsSetController : Controller
    {
        private readonly string ScanImagePath = @".\wwwroot\Sealcard\";
        private readonly string SealTempPath = Path.GetTempPath() + @"Seal\";
        private readonly float HeightScale = 0.25f;
        private readonly float WidthScale = 0.25f;        

        /// <summary>
        /// 印鑑建檔頁面 讀取資料夾所有圖檔
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<ImageData> imageDatas = GetImages(ScanImagePath);
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
            List<ImageData> sealImage = GetImages(SealTempPath);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public IActionResult GetSealImage(string imageName)
        {
            ImageData? imageData = GetImageData(SealTempPath, imageName);
            if (imageData != null)
            {
                //image.IsSelected = true;
                //ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(image.Data, 0, image.Data.Length);
                return File(imageData.Data, imageData.ContentType);
            }
            return View("Error");
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
        /// 取得圖檔並顯示指定的圖
        /// </summary>
        /// <param name="imageName">圖片檔名</param>
        /// <returns></returns>
        public IActionResult GetScanImage(string imageName)
        {
            ImageData? imageData = GetImageData(ScanImagePath,imageName);            
            if (imageData != null)
            {
                //image.IsSelected = true;
                //ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(image.Data, 0, image.Data.Length);
                return File(imageData.Data,imageData.ContentType);
            }
            return View("Error");
        }

        /// <summary>
        /// 取得指定檔名圖檔資料
        /// </summary>
        /// <param name="imagePath">圖檔位置</param>
        /// <param name="imageName">圖片檔名</param>
        /// <returns></returns>
        public ImageData? GetImageData(string imagePath, string imageName)
        {

            ImageData imageData = new()
            {
                FileName = imageName,
                ContentType = "imageData/" + imageName.IndexOf(".") + 1,
                Data = ImageResize.ReDrawImgToBytes(imagePath + imageName, WidthScale, HeightScale),//縮放圖檔並轉成Bytes
            };
            if (imageData.Data != null)
            {
                return imageData;                
            }
            else
            {
                return null;
            }            
        }

        /// <summary>
        /// 獲得資料夾內的所有圖檔以及圖檔的檔名
        /// </summary>
        /// <param name="imageath">圖檔路徑</param>
        /// <param name="imageType">圖檔型態</param>
        /// <returns></returns>
        private static List<ImageData> GetImages(string imagePath)
        {
            List<ImageData> images = new();
            string[] files = Directory.GetFiles(imagePath);
            if (files.Length > 0)
            {
                DirectoryInfo folder = new(imagePath);
                //指定讀取資料夾內的圖檔type
                foreach (FileInfo file in folder.GetFiles("*.*"))
                {                    
                    int extensionLocation = file.Name.IndexOf(".") + 1;
                    images.Add(new ImageData
                    {
                        FileName = file.Name,
                        FileFullName = file.FullName,
                        ContentType = "imageData/" + file.Name[extensionLocation..],
                        //Data = ImageResize.ImgToBytes(imagePath + file.Name, WidthScale, HeightScale),//縮放圖檔並轉成Bytes
                    });
                }
            }
            return images;
        }
    }
}