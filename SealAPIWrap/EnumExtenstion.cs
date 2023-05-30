using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SealAPIWrap
{
    /// <summary>
    /// Enum 擴充
    /// </summary>
    public static class EnumExtenstion
    {
        /// <summary>
        /// 取得描述屬性資料
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            return !(fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute descriptionAttribute) ? 
                value.ToString() : descriptionAttribute.Description;
        }
    }
}
