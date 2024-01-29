using DJSpire.Models;
using Spire.Xls;

namespace DJSpire.Utils
{
    public class ExcelUtil
    {
        /// <summary>
        /// 創建Excel檔案
        /// </summary>
        /// <typeparam name="T"></typeparam>        
        /// <param name="excelData"></param>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        public MemoryStream CreateFile<T>(ExcelData<T> excelData, FileFormat fileFormat = FileFormat.Version2016) where T : class
        {
            MemoryStream result = new();
            Workbook workbook = new();
            workbook.CreateEmptySheets(1);
            Worksheet sheet = workbook.Worksheets[0];

            try
            {
                for (int row = 1; row <= excelData.Values.Count; row++)
                {
                    for (int col = 1; col <= typeof(T).GetProperties().Length; col++)
                    {
                        // 使用反射取得屬性名稱，然後取得該屬性的值
                        string propertyName = GetPropertyName<T>(col - 1);
                        object? propertyValue = typeof(T).GetProperty(propertyName)?.GetValue(excelData.Values[row - 1]);

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

        /// <summary>
        /// 取得class名稱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string GetPropertyName<T>(int index)
        {
            return typeof(T).GetProperties()[index].Name;
        }
    }
}
