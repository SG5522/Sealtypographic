using OpenCvSharp;

namespace DJLib
{
    /// <summary>
    /// 圖片處理
    /// </summary>
    public class OpenCvUtil
    {
        public static void Transparent(string path)
        {
            //Mat outImage = new Mat();
            Mat sourceImage = Cv2.ImRead(path);            
            //Cv2.CvtColor(sourceImage, outImage, ColorConversionCodes.BGR2BGRA);
            Mat maskMat = GetMaskMat(sourceImage, 160);
            //MergeMask(sourceImage, outImage, maskMat);
            maskMat.SaveImage(@"D:\Temp\123.png");
            //Mat[] mats = outImage.Split();
        }

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
    }
}
