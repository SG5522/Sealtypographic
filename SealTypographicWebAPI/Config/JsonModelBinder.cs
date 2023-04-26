using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// BindModel
    /// </summary>
    public class JsonModelBinder : IModelBinder
    {
        /// <summary>
        /// 使用FormForm時，複雜的class模型需要用自定義的ModelBinding才能正常輸入。
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Check the value provider for the JSON string
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {                
                JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
                options.Converters.Add(new JsonStringEnumConverter());
                if(valueProviderResult.FirstValue != null)
                {
                    // Deserialize the JSON string to the desired object type
                    object? deserializedValue = JsonSerializer.Deserialize(valueProviderResult.FirstValue, bindingContext.ModelType, options);

                    // Set the result of the model binding operation
                    bindingContext.Result = ModelBindingResult.Success(deserializedValue);                    
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex.Message);
                return Task.CompletedTask;
            }
        }
    }
}
