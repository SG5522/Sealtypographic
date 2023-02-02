using System.ComponentModel;
using System.Reflection;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// Enum的擴充
    /// </summary>
    public static class EnumExtenstion
    {
        /// <summary>
        /// 回傳 Enum 的 Description 屬性，如果沒有 Description 屬性就回傳列舉成員名稱
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            DescriptionAttribute? attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
