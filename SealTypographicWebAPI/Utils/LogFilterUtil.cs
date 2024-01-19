using System.Collections;
using System.Reflection;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 過濾不必要的log資料
    /// </summary>
    public class LogFilterUtil
    {
        /// <summary>
        /// 過濾，一定過濾 ImageBase64
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T? FilterSensitiveData<T>(T obj)
        {
            return FilterSensitiveData(obj, new List<string> { "ImageBase64" });
        }

        /// <summary>
        /// 過濾，指定要過濾的屬性名稱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertiesToFilter">要過濾的屬性名稱清單</param>
        /// <returns></returns>
        public static T? FilterSensitiveData<T>(T obj, List<string> propertiesToFilter)
        {
            T? result = default;
            if (obj != null)
            {
                result = (T)FilterObject(obj, propertiesToFilter);
            }
            return result;
        }

        private static object FilterObject(object obj, List<string> propertiesToFilter)
        {
            object result;
            Type objectType = obj.GetType();                        

            if (objectType.IsClass && !objectType.IsPrimitive && objectType != typeof(string))
            {
                // 建立新的物件，複製原始物件的值
                result = Activator.CreateInstance(objectType)!;

                foreach (PropertyInfo property in objectType.GetProperties())
                {
                    if (ShouldFilterProperty(property, propertiesToFilter))
                    {
                        property.SetValue(result, "Filtered");
                    }
                    else
                    {
                        object? propertyValue = property.GetValue(obj);
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            // 如果是泛型 List
                            Type listType = property.PropertyType.GetGenericArguments()[0];
                            if (listType.IsClass && propertyValue is IList list)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    list[i] = FilterObject(list[i]!, propertiesToFilter);
                                }
                            }
                        }
                        else if (property.PropertyType.IsClass && propertyValue != null)
                        {
                            // 遞迴處理巢狀類別
                            propertyValue = FilterObject(propertyValue, propertiesToFilter);
                            property.SetValue(result, propertyValue);
                        }
                    }
                }
            }
            else
            {
                result = obj;
            }

            return result;
        }

        private static bool ShouldFilterProperty(PropertyInfo property, List<string> propertiesToFilter)
        {
            // 是否在要過濾的屬性名稱清單中 (不分大小寫)
            return propertiesToFilter.Contains(property.Name, StringComparer.OrdinalIgnoreCase) && property.CanWrite;
        }

    }
}
