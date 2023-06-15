using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Text.RegularExpressions;
using System.IO;

namespace DJLib.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 圖片
        /// </summary>
        public Image SourceImage { get; set; }

        /// <summary>
        /// 圖片格式
        /// </summary>
        public IImageFormat ImageFormat { get; set; }

        

        /// <summary>
        /// ImageBase64 含","前面的文字(例 data:image/png;base64) 轉ImageInfo
        /// </summary>
        /// <param name="ImageBase64"></param>
        /// <returns></returns>
        public static ImageInfo FromImageBase64(string ImageBase64)
        {            
            //return FromBase64(ImageBase64.Substring(ImageBase64.IndexOf("base64,") + 7));
            return FromBase64(Regex.Replace(ImageBase64, @"^data:image\/[a-zA-Z]+;base64,", string.Empty));
        }

        /// <summary>
        /// Base64不含","前面的文字(例 data:image/png;base64,拿掉) 轉ImageInfo
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static ImageInfo FromBase64(string base64String)
        {
            return new ImageInfo()
            {
                SourceImage = Image.Load(Convert.FromBase64String(base64String), out IImageFormat format),
                ImageFormat = format
            };
        }

        public static ImageInfo FromPath(string path)
        {
            return new ImageInfo()
            {
                SourceImage = Image.Load(path, out IImageFormat format),
                ImageFormat = format
            };
        }

        /// <summary>
        /// 調整圖片大小(Image)
        /// </summary>
        /// <param name="imageInfo">圖片資訊</param>
        /// <param name="scale">縮放比例 1.00 = 100%  0.01 = 1%</param>        
        public void ReSize(ImageInfo imageInfo, double scale)
        {
            int width = (int)(imageInfo.SourceImage.Width * scale);
            int height = (int)(imageInfo.SourceImage.Height * scale);
            imageInfo.SourceImage.Mutate(x => x.Resize(width, height));
        }

        /// <summary>
        /// 白色透明化
        /// </summary>
        /// <param name="threshold">臨界點</param>
        public Stream Transparent(int threshold = 160)
        {
            Stream stream = new MemoryStream();
            SourceImage.Save(stream,ImageFormat);            
            return OpenCvUtil.TransparentToStream(stream, threshold);
        }

        public string ImageToBase64()
        {
            return SourceImage.ToBase64String(ImageFormat);
        }
    }
}
