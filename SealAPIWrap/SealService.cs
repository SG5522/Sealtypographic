using SealAPIWrap.Models;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace SealAPIWrap
{
    /// <inheritdoc/>
    public class SealService : ISealService
    {
        /// <inheritdoc/>
        public K Operation<T, K>(OPMode opMode, T form) where T : ApiRequest where K : ApiResult
        {
            K result = Activator.CreateInstance<K>();

            try
            {
                string inputString = JsonSerializer.Serialize(form);
                
                //利用Method Invoke取得ApiWrap要執行的Method資訊
                MethodInfo methodInfo = typeof(ApiWrap).GetMethod(opMode.ToString());
                IntPtr? returnIntPtr;
                if (opMode == OPMode.ImageProcess || opMode == OPMode.SealShow)
                {
                    returnIntPtr = methodInfo.Invoke(typeof(ApiWrap), new object[] { inputString }) as IntPtr?;
                }
                else
                {
                    returnIntPtr = methodInfo.Invoke(typeof(ApiWrap), new object[] { ApiWrap.DEFAULT_MODID, inputString }) as IntPtr?;
                }

                result = JsonSerializer.Deserialize<K>(Marshal.PtrToStringAnsi(returnIntPtr.Value));
                if (result.RetCode != ApiWrap.RETURN_SUCCESS)
                {
                    result.ErrorMessage = ((ErrorMsg)result.RetCode).GetDescription();
                }
            }
            catch (Exception ex)
            {
                result.RetCode = ApiWrap.RETURN_FIALURE;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
