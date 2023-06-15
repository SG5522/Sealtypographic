using DJLib.Consts;
using OpenCvSharp;
using SixLabors.ImageSharp.Drawing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace DJLib
{
    /// <summary>
    /// 圖片處理
    /// </summary>
    public class OpenCvUtil
    {
        /// <summary>
        /// 圖片白色底圖透通(輸入路徑)回傳流
        /// </summary>
        /// <param name="path">路徑</param>
        public static Stream TransparentToStream(string path, int threshold)
        {            
           return TransparentByWhite(Cv2.ImRead(path), threshold).ToMemoryStream();
        }

        /// <summary>
        /// 圖片白色底圖透通(輸入路徑)回傳流
        /// </summary>
        /// <param name="stream">流</param>
        public static Stream TransparentToStream(Stream stream, int threshold)
        {
            return TransparentByWhite(Mat.FromStream(stream, ImreadModes.AnyColor), threshold).ToMemoryStream();
        }

        /// <summary>
        /// 圖片白色底圖透通(輸入路徑)回傳流
        /// </summary>
        /// <param name="srcMat">OpenCv的Mat Class</param>
        public static Stream TransparentToStream(Mat srcMat, int threshold)
        {                   
            return TransparentByWhite(srcMat, threshold).ToMemoryStream();
        }

        /// <summary>
        /// 圖片白色底圖透通(輸入路徑)回傳bytes
        /// </summary>
        /// <param name="path">路徑</param>
        public static byte[] TransparentToBytes(string path, int threshold)
        {
            return TransparentByWhite(Cv2.ImRead(path), threshold).ToBytes();
        }

        /// <summary>
        /// 圖片白色底圖透通(輸入流)回傳bytes
        /// </summary>
        /// <param name="stream">流</param>
        public static byte[] TransparentToBytes(Stream stream, int threshold)
        {
            return TransparentByWhite(Mat.FromStream(stream, ImreadModes.AnyColor), threshold).ToBytes();
        }

        /// <summary>
        /// 圖片白色底圖透通(輸入Mat)回傳bytes
        /// </summary>
        /// <param name="srcMat"></param>
        public static byte[] TransparentToBytes(Mat srcMat, int threshold)
        {            
            return TransparentByWhite(srcMat, threshold).ToBytes();
        }

        /// <summary>
        /// 白色底圖透通處理(回傳Mat)
        /// 使用Opencv的Foreach處理
        /// </summary>
        /// <param name="srcMat"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static Mat TransparentByWhiteForEach(Mat srcMat, int threshold)
        {
            Mat tempMat = srcMat.CvtColor(ColorConversionCodes.BGR2BGRA);
            unsafe
            {
                tempMat.ForEachAsVec4b((ptrValue, ptrPosition) =>
                {
                    if (ptrValue->Item0 > threshold && ptrValue->Item1 > threshold && ptrValue->Item2 > threshold)
                    {
                        ptrValue->Item3 = 0;
                    }
                });
            }
            return tempMat;
        }

        /// <summary>
        /// 白色底圖透通處理(回傳Mat)
        /// </summary>
        /// <param name="srcMat"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static Mat TransparentByWhite(Mat srcMat, int threshold)
        {
            Mat tempMat = srcMat.CvtColor(ColorConversionCodes.BGR2BGRA);

            for (int row = 0; row < tempMat.Rows; row++)
            {
                for (int col = 0; col < tempMat.Cols; col++)
                {
                    Vec4b vec4b = tempMat.Get<Vec4b>(row, col);
                    if (vec4b.Item0 > threshold && vec4b.Item1 > threshold && vec4b.Item2 > threshold)
                    {
                        vec4b.Item3 = 0;
                        tempMat.Set(row, col, vec4b);
                    }
                }
            }
            return tempMat;
        }

        /// <summary>
        /// 以紅色設定範圍將紅色以外的顏色變為黑色(二值化)
        /// </summary>
        /// <param name="srcMat">來源圖</param>
        /// <returns></returns>
        public static Mat GetBinaryMat(Mat srcMat, BinaryColor binaryColor)
        {
            //定義顏色範圍
            Scalar scalarLower;
            Scalar scalaRupper;

            //各項顏色範圍尚未測試
            switch (binaryColor)
            {
                case BinaryColor.Red:
                    scalarLower = new Scalar(0, 0, 150);
                    scalaRupper = new Scalar(80, 80, 255);
                    break;
                case BinaryColor.Blue:
                    scalarLower = new Scalar(100, 0, 0);
                    scalaRupper = new Scalar(255, 50, 50);
                    break;
                case BinaryColor.Black:
                    scalarLower = new Scalar(0, 0, 0);
                    scalaRupper = new Scalar(30, 30, 30);
                    break;
                default:
                    scalarLower = new Scalar(0, 0, 150);
                    scalaRupper = new Scalar(80, 80, 255);
                    break;
            }

            //提取範圍內的像素
            Mat mask = srcMat.InRange(scalarLower, scalaRupper);            
          
            // 二值化處理
            return srcMat.BitwiseAnd(mask);
        }

        /// <summary>
        /// 差補點
        /// </summary>
        /// <param name="srcMat">來源圖</param>
        /// <returns></returns>
        public static Mat Inpaint(Mat srcMat)
        {
            // 建立一個全白色的遮罩，表示整個影像都要修補
            Mat mask = Mat.Zeros(srcMat.Rows, srcMat.Cols, MatType.CV_8UC1);
            mask.SetTo(255);

            // 使用差補點算法進行影像修補
            Mat inpaintedImage = new Mat();
            Cv2.Inpaint(srcMat, mask, inpaintedImage, 3, InpaintMethod.Telea);            
            return inpaintedImage;
        }
    }
}
