using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System;

namespace DJLib
{
    public class ImageResize
    {
        /// <summary>
        /// 圖像縮放
        /// </summary>
        /// <param name="image"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Bitmap ResizedImg(string path, float WidthScale, float HeightScale)
        {
            FileStream fs = File.OpenRead(path);
            Image image = Image.FromStream(fs);
            int width = (int)(image.Width * WidthScale);
            int height = (int)(image.Height * HeightScale);
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            image.Dispose();
            fs.Dispose();
            return destImage;
        }

        /// <summary>
        /// 圖檔縮放並轉成Bytes
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <param name="WidthScale">寬縮放大小(float)</param>
        /// <param name="HeightScale">高縮放大小(float)</param>
        /// <returns></returns>
        public static byte[] ResizedImgToBytes(string path, float WidthScale, float HeightScale)
        {
            MemoryStream ms = new MemoryStream();
            
            Bitmap destImage = ResizedImg(path, WidthScale, HeightScale);            
            destImage.Save(ms, ImageFormat.Png);

            byte[] imageBytes = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(imageBytes, 0, (int)ms.Length);
            ms.Close();

            destImage.Dispose();
            return imageBytes;
        }
    }
}
