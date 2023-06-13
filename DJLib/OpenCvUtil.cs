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
            Mat outImage = new Mat();
            Mat sourceImage = Cv2.ImRead(path);            
            Cv2.CvtColor(sourceImage, outImage, ColorConversionCodes.BGR2BGRA);
            Mat maskMat =  GetMaskMat(outImage, 235);
            MergeMask(sourceImage, outImage, maskMat);
            outImage.SaveImage("C://123.png");
            //Mat[] mats = outImage.Split();
        }

        private static unsafe Mat GetMaskMat(Mat src, int threshold)
        {
            Mat mat = new Mat(src.Rows, src.Cols, MatType.CV_8UC4);
            for(int row = 0; row < src.Rows; row++)
            {
                Vec4b* srcRow = (Vec4b*)src.Ptr(row);
                Vec4b* maskRow = (Vec4b*)mat.Ptr(row);
                for(int col = 0; col < src.Cols; col++)
                {
                    Vec4b* pData = srcRow + col;
                    Vec4b* maskPData = maskRow + col;

                    byte blue = pData->Item0;
                    byte green = pData->Item1;
                    byte red = pData->Item2;
                    byte alpha = pData->Item3;
                    
                    if (blue <= threshold || green <= threshold || red <= threshold)
                    {
                        
                        maskPData-> Item0 = 255;
                        maskPData-> Item1 = 255;
                        maskPData-> Item2 = 255;
                        maskPData-> Item3 = 255;
                    }
                    else
                    {                        
                        maskPData->Item0 = 0;
                        maskPData->Item1 = 0;
                        maskPData->Item2 = 0;
                        maskPData->Item3 = 0;
                    }
                }
            }
            return mat;
        }

        private static void MergeMask(Mat sourceImage, Mat outImage, Mat maskMat)
        {            
            Mat[] sourceImages = sourceImage.Split();
            Mat[] maskMats = maskMat.Split();
            Mat[] newMats = new Mat[] { sourceImages[0], sourceImages[1], sourceImages[2], maskMats[3] };
            Cv2.Merge(newMats, outImage);
        }
    }
}
