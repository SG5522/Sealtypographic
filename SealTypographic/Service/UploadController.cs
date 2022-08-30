using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographic.Models;
using System.Text.RegularExpressions;

namespace SealTypographic.Service
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public string UploadFiles(List<ImageUploadData> imageUploadDatas)
        {            
            try
            {
                foreach(ImageUploadData imageUploadData in imageUploadDatas)
                {
                    string fileName = Path.GetFileName(imageUploadData.FileName);
                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UploadFiles", fileName);
                    string imageBase64 = Regex.Replace(imageUploadData.ImageBase64, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
                    byte[] bytes = Convert.FromBase64String(imageBase64);
                    System.IO.File.WriteAllBytes(uploadpath, bytes);
                }

                var json = new
                {
                    message = "File uploaded successfully.",
                    status = 1
                };
                return JsonConvert.SerializeObject(json);
            }
            catch
            {
                var json = new
                {
                    message = "Error while uploading the files.",
                    status = 0
                };
                return JsonConvert.SerializeObject(json);
            }
        }
    }
}
