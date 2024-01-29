
namespace DJSpire.Models
{
    public class ExcelData<T>
    {
        /// <summary>
        /// 建制時 Hearders的List不得超過T的屬性量
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="values"></param>
        /// <exception cref="ArgumentException"></exception>
        public ExcelData(List<string>? headers, List<T> values)
        {
            if (headers != null && headers.Count > 0 && values.Count > 0)
            {                
                if (headers.Count > typeof(T).GetProperties().Length)
                {
                    throw new ArgumentException("Headers count cannot exceed the number of properties in Data.");
                }
            }

            Headers = headers;
            Values = values;
        }

        public List<string>? Headers { get; set; }
        public List<T> Values { get; set; }
    }
}
