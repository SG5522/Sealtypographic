using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using System;
using SixLabors.ImageSharp.Formats.Png;

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
            return FromBase64(ImageBase64.Substring(ImageBase64.IndexOf("base64,") + 7));
        }

        /// <summary>
        /// Base64不含","前面的文字(例 data:image/png;base64,拿掉) 轉ImageInfo
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static ImageInfo FromBase64(string base64String)
        {
            //ImageInfo imageInfo = new ImageInfo();
            //byte[] bytes = Convert.FromBase64String(base64String);
            //imageInfo.Image = Image.Load(bytes, out IImageFormat format);
            //imageInfo.ImageFormat = format;
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
            PngEncoder encoder = new PngEncoder()
            {
                ColorType = PngColorType.RgbWithAlpha,
                TransparentColorMode = PngTransparentColorMode.Preserve,
                BitDepth = PngBitDepth.Bit8,
                CompressionLevel = PngCompressionLevel.BestSpeed
            };

            RecolorBrush brush = new RecolorBrush(Color.White, Color.Transparent, threshold);      
            

            Image.Mutate(x =>
            {
                x.Fill(brush,);
            });


            Image.SaveAsPng("C:\\123.png", encoder);            
                            //x.Clear(brush));            
        }

        public string ImageToBase64()
        {
            return Image.ToBase64String(ImageFormat);
        }
    }
}
