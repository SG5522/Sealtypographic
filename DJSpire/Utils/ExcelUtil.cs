using DJSpire.Models;
using Spire.Xls;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DJSpire.Utils
{
    public class ExcelUtil
    {
        /// <summary>
        /// 存在指定路徑
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelData"></param>
        /// <param name="filePath"></param>
        /// <param name="fileFormat"></param>
        public static void CreateFileToSavePath<T>(ExcelData<T> excelData, string filePath, FileFormat fileFormat = FileFormat.Version2016) where T : class
        {
            using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);
            CreateFile(excelData, fileFormat).WriteTo(fileStream);
        }

        /// <summary>
        /// 創建Excel檔案 Base64輸出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelData"></param>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        public static string CreateFileToBase64<T>(ExcelData<T> excelData, FileFormat fileFormat = FileFormat.Version2016) where T : class
        {
            return Convert.ToBase64String(CreateFileToBytes(excelData, fileFormat));
        }

        /// <summary>
        /// 創建Excel檔案 Bytes輸出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelData"></param>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        public static byte[] CreateFileToBytes<T>(ExcelData<T> excelData, FileFormat fileFormat = FileFormat.Version2016) where T : class
        {
            return CreateFile(excelData, fileFormat).ToArray();
        }

        /// <summary>
        /// 創建Excel檔案
        /// </summary>
        /// <typeparam name="T"></typeparam>        
        /// <param name="excelData"></param>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        public static MemoryStream CreateFile<T>(ExcelData<T> excelData, FileFormat fileFormat = FileFormat.Version2016) where T : class
        {
            MemoryStream result = new();
            Workbook workbook = new();
            workbook.CreateEmptySheets(1);
            Worksheet sheet = workbook.Worksheets[0];

            try
            {                
                PropertyInfo[] properties = typeof(T).GetProperties()
                                            .OrderBy(
                                            p => {
                                                return p.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() is DisplayAttribute displayAttribute ? displayAttribute.Order : int.MaxValue;
                                            })
                                            .ToArray();

                for (int row = 1; row <= excelData.Values.Count; row++)
                {
                    for (int col = 1; col <= properties.Length; col++)
                    {
                        // 使用反射取得屬性名稱，然後取得該屬性的值                     
                        object? propertyValue = properties[col - 1].GetValue(excelData.Values[row - 1]);

                        if (excelData.Headers != null)
                        {
                            //塞入標頭用
                            if (row == 1)
                            {
                                sheet.Range[row, col].Value = excelData.Headers[col - 1];
                            }
                            sheet.Range[row + 1, col].Value = propertyValue != null ? Convert.ToString(propertyValue) : null;
                        }
                        else
                        {
                            sheet.Range[row, col].Value = propertyValue != null ? Convert.ToString(propertyValue) : null;
                        }
                    }
                }
                sheet.AllocatedRange.AutoFitColumns();
                workbook.SaveToStream(result, fileFormat);                
            }
            catch (InvalidCastException) 
            {
                throw;
            }            
            return result;
        }
    }
}
