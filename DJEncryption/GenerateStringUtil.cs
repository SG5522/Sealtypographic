using System;
using System.Text;

namespace DJEncryption
{
    public class GenerateStringUtil
    {
        public const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; // 可以使用的字符

        /// <summary>
        /// 取得隨機字串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {            
            Random randon = new Random((int)(DateTime.Now.Ticks));            
            StringBuilder passwordBuilder = new StringBuilder(length);
            for (int i = 0; i <= length; i++)
            {
                passwordBuilder.Append(ValidChars[randon.Next(0,length)]);
            }
            return passwordBuilder.ToString();
        }
    }
}
