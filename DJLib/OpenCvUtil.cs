using OpenCvSharp;
using System;

namespace DJLib
{
    /// <summary>
    /// 圖片處理
    /// </summary>
    public class OpenCvUtil
    {
        public static void Transparent(string path)
        {
            
            //Mat tmpMat = new Mat();
            //Mat alphaMat = new Mat();
            Mat sourceImage = Cv2.ImRead(path);
            Mat outMat = GetMat(sourceImage, 150);
            outMat.SaveImage("C://123.png");            
        }

        private static Mat GetMat(Mat srcMat,int threshold)
        {
            Mat tempMat = srcMat.CvtColor(ColorConversionCodes.BGR2BGRA);
            //Mat[] splitMats = tempMat.Split();

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
    }
}
