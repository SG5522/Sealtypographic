using OpenCvSharp;
using SixLabors.ImageSharp.Drawing;
using System.IO;

namespace DJLib
{
    /// <summary>
    /// 圖片處理
    /// </summary>
    public class OpenCvUtil
    {
        /// <summary>
        /// 透明化(輸入路徑)
        /// </summary>
        /// <param name="path">路徑</param>
        public static void Transparent(string path)
        {            
            TransparentProcess(Cv2.ImRead(path));
        }

        /// <summary>
        /// 透明化(輸入流)
        /// </summary>
        /// <param name="stream">流</param>
        public static void Transparent(Stream stream)
        {            
            TransparentProcess(Mat.FromStream(stream, ImreadModes.AnyColor));
        }

        /// <summary>
        /// 圖片透通
        /// </summary>
        /// <param name="srcMat"></param>
        private static void TransparentProcess(Mat srcMat)
        {
            Mat maskMat = GetMaskMat(srcMat, 160);
            maskMat.SaveImage(@"D:\123.png");
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="srcMat"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private static Mat GetMaskMatForEach(Mat srcMat, int threshold)
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
        /// 
        /// </summary>
        /// <param name="srcMat"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private static Mat GetMaskMat(Mat srcMat, int threshold)
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

        private static Mat GetMat2(Mat srcMat)
        {
            //定義紅色範圍
            Scalar lowerRed = new Scalar(0, 0, 150);
            Scalar upperRed = new Scalar(80, 80, 255);

            // 提取紅色範圍內的像素
            Mat redMask = new Mat();
            Cv2.InRange(srcMat, lowerRed, upperRed, redMask);

            // 透明化非紅色範圍的像素
            Mat MaskResult = new Mat();
            Cv2.BitwiseAnd(srcMat, srcMat, MaskResult, redMask);
            
            return GetMaskMat(MaskResult, 255);
        }
    }
}
