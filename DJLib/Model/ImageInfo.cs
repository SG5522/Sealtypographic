using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System;

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
        /// ImageBase64 含","前面的文字(例 data:image/png;base64) 轉ImageInfo
        /// </summary>
        /// <param name="ImageBase64"></param>
        /// <returns></returns>
        public static ImageInfo FromImageBase64(string ImageBase64)
        {
            string base64String = ImageBase64.Substring(ImageBase64.IndexOf("base64,") + 7);
            return FromBase64(base64String);
        }

        /// <summary>
        /// Base64不含","前面的文字(例 data:image/png;base64,拿掉) 轉ImageInfo
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static ImageInfo FromBase64(string base64String)
        {
            ImageInfo imageInfo = new ImageInfo();
            byte[] bytes = Convert.FromBase64String(base64String);
            imageInfo.Image = Image.Load(bytes, out IImageFormat format);
            imageInfo.ImageFormat = format;

            return imageInfo;
        }

        /// <summary>
        /// 調整圖片大小(Image)
        /// </summary>
        /// <param name="imageInfo">圖片資訊</param>
        /// <param name="scale">縮放比例 1.00 = 100%  0.01 = 1%</param>        
        public static void ReSize(ImageInfo imageInfo, double scale)
        {
            int width = (int)(imageInfo.Image.Width * scale);
            int height = (int)(imageInfo.Image.Height * scale);
            imageInfo.Image.Mutate(x => x.Resize(width, height));
        }

        public static void Transparent(ImageInfo imageInfo)
        {
            float threshold = 0.5F;
            Color sourceColor = Color.White;
            Color targetColor = Color.Transparent;
            RecolorBrush brush = new RecolorBrush(sourceColor, targetColor, threshold);
            imageInfo.Image.Mutate
                        (
                            //x => x.Fill(graphicsOptions,brush,)
                            x => x.Clear(brush)
                        );
        }
    }
}
