using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Text.RegularExpressions;

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
        public Image Image { get; set; }

        /// <summary>
        /// 圖片格式
        /// </summary>
        public IImageFormat ImageFormat { get; set; }

        /// <summary>
        /// 包含alpha值的Image
        /// </summary>
        public Image<Rgba32> ImageRGBA32 { get; set; }

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
                Image = Image.Load(Convert.FromBase64String(base64String), out IImageFormat format),
                ImageFormat = format
            };
        }

        public static ImageInfo FromPath(string path)
        {
            return new ImageInfo()
            {
                Image = Image.Load(path, out IImageFormat format),
                ImageFormat = format
            };
        }

        public static ImageInfo FromPathWithAlpha(string path)
        {
            return new ImageInfo()
            {
                ImageRGBA32 = Image.Load(path, out IImageFormat format),
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
            int width = (int)(imageInfo.Image.Width * scale);
            int height = (int)(imageInfo.Image.Height * scale);
            imageInfo.Image.Mutate(x => x.Resize(width, height));
        }

        /// <summary>
        /// 白色透明化
        /// </summary>
        /// <param name="threshold">臨界點</param>
        public void Transparent(float threshold = 0.1F)
        {
            RecolorBrush brush = new RecolorBrush(Color.White, Color.Transparent, threshold);
            ImageRGBA32 = Image

            byte[] bytes = Convert.FromBase64String(Regex.Replace(Image.ToBase64String(ImageFormat), @"^data:image\/[a-zA-Z]+;base64,", string.Empty));
            Image<Rgba32> image = (Image<Rgba32>)Image.Load(bytes);            
            image.Mutate(x =>
            {
                x.Fill(brush);                
            });


            image.Save("C:\\123.png");

            //Image.SaveAsPng("C:\\123.png", encoder);            
            //x.Clear(brush));            
            //Image.SaveAsPng("C:\\123.png");
        }

        public string ImageToBase64()
        {
            return Image.ToBase64String(ImageFormat);
        }
    }
}
