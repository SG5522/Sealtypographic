using Microsoft.AspNetCore.Mvc;
using DJLib;
using SealTypographic.Models;
using TchznSeal;
using Newtonsoft.Json;

namespace SealTypographic.Controllers
{
    public class CreateSealsSetController : Controller
    {
        private readonly string scanImagePath = @"D:\works\SealTypographic\SealTypographic\wwwroot\Sealcard\";        
        private readonly float HeightScale = 0.25f;
        private readonly float WidthScale = 0.25f;
        private AutoSealSplit autoSealSplit = new();

        /// <summary>
        /// 印鑑建檔頁面 讀取資料夾所有圖檔
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<Image> imageDatas = GetImages(scanImagePath, "jpg");
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
            List<Image> sealImage = GetImages(Path.GetTempPath() + @"\Seal\","bmp");
            int count = 1;
            //@呼叫天創元件進行分離 每次會自己產生bmp檔 後續需要請天創調整元件
            autoSealSplit.SealSplit(scanImagePath + imageName , Path.GetTempPath() + @"\Seal\", "Test", "R");            
            foreach(Image image in sealImage)
            {                
                seals.Add(new Seal
                {
                    SealImageFullName = image.FileFullName,
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
        /// 取得圖檔並顯示指定的圖
        /// </summary>
        /// <param name="imageName">圖片檔名</param>
        /// <returns></returns>
        public IActionResult GetImage(string imageName)
        {
            Image? image = GetSealImage(imageName);
            if (image != null)
            {
                //image.IsSelected = true;
                //ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(image.Data, 0, image.Data.Length);
                return File(image.Data,image.ContentType);
            }
            return View("Error");
        }

        /// <summary>
        /// 取得指定檔名圖檔資料
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public Image? GetSealImage(string imageName)
        {
            List<Image> images = GetImages(scanImagePath,"jpg");
            return images.Find(p => p.FileName == imageName);            
        }

        /// <summary>
        /// 獲得資料夾內的所有圖檔以及圖檔的檔名 資料型態(目前先固定為jpg) byte[]
        /// </summary>
        /// <param name="imageath">圖檔路徑</param>
        /// <param name="imageType">圖檔型態</param>
        /// <returns></returns>
        private List<Image> GetImages(string imageath,string imageType)
        {
            List<Image> images = new();
            string[] files = Directory.GetFiles(imageath);
            if (files.Length > 0)
            {
                DirectoryInfo folder = new(imageath);
                foreach (FileInfo file in folder.GetFiles("*."+ imageType))
                {
                    images.Add(new Image
                    {
                        FileName = file.Name,
                        FileFullName = file.FullName,
                        ContentType = "image/jpg",
                        Data = ImageResize.ResizedImgToBytes(imageath + file.Name, WidthScale, HeightScale),//縮放圖檔並轉成Bytes
                    });
                }
            }
            return images;
        }
    }
}
/*
@if (ViewBag.Base64String != null)
{
    <img id="scanimage" src="@ViewBag.Base64String" class="img-fluid" alt="Responsive image" style="width: auto; height: auto;"/>
}
else
{
    <img id="scanimage" src="img/NoImage.svg" class="img-fluid" alt="Responsive image" style="width: auto; height: auto;"/>
}
public byte[]? GetBytesFromImage(string imagePath)
{
    try
    {
        FileStream fs = new(imagePath, FileMode.Open, FileAccess.Read);
        int length = (int)fs.Length;
        byte[] image = new byte[length];
        fs.Read(image, 0, length);
        fs.Close();
        return image;
    }
    catch (Exception)
    {
        return null;
    }
}
 */