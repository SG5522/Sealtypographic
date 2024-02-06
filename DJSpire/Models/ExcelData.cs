using System.Diagnostics;

namespace DJSpire.Models
{
    public class ExcelData<T>
    {
        /// <summary>
        /// 建制時 Headers 的 List 可以超過 T 的屬性量，但會進行警告
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="values"></param>
        /// <exception cref="ArgumentException"></exception>
        public ExcelData(List<T> values, List<string>? headers = null)
        {
            Debug.Assert(headers == null || headers.Count <= typeof(T).GetProperties().Length,
            "Headers count exceeds the number of properties in Data.");

            Headers = headers;
            Values = values;
        }

        public List<string>? Headers { get; set; }
        public List<T> Values { get; set; }
    }
}
