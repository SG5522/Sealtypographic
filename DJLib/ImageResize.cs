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
        /// <param name="fullname"></param>
        /// <param name="widthScale"></param>
        /// <param name="heightScale"></param>
        /// <returns></returns>
        public static Bitmap ResizedImg(string fullname, float widthScale, float heightScale)
        {
            FileStream fileStream = File.OpenRead(fullname);
            Image image = Image.FromStream(fileStream);
            int width = (int)(image.Width * widthScale);
            int height = (int)(image.Height * heightScale);
            Rectangle targatRect = new Rectangle(0, 0, width, height);
            Bitmap targatBitmap = new Bitmap(width, height);

            targatBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            Graphics graphics = Graphics.FromImage(targatBitmap);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            ImageAttributes wrapMode = new ImageAttributes();

            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, targatRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            
            wrapMode.Dispose();
            graphics.Dispose();
            image.Dispose();
            fileStream.Dispose();

            return targatBitmap;
        }

        /// <summary>
        /// 圖檔縮放並轉成Bytes
        /// </summary>
        /// <param name="fullName">檔案路徑</param>
        /// <param name="widthScale">寬縮放大小(float)</param>
        /// <param name="heightScale">高縮放大小(float)</param>
        /// <returns></returns>
        public static byte[] ResizedImgToBytes(string fullName, float widthScale, float heightScale)
        {
            MemoryStream memoryStream = new MemoryStream();
            
            Bitmap targatBitmap = ResizedImg(fullName, widthScale, heightScale);            
            targatBitmap.Save(memoryStream, ImageFormat.Png);

            byte[] imageBytes = new byte[memoryStream.Length];
            memoryStream.Position = 0;
            memoryStream.Read(imageBytes, 0, (int)memoryStream.Length);
            memoryStream.Close();

            targatBitmap.Dispose();
            return imageBytes;
        }
    }
}
