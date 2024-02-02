using CommonLib.Extensions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name =>
                    {
                        Enum e = (Enum)Enum.Parse(context.Type, name);
                        string enumDescription = e.GetDescription();
                        if (name.Equals(enumDescription))
                        {
                            schema.Enum.Add(new OpenApiString($"{name} = {Convert.ToInt64(e)}"));
                            //schema.Enum.Add(new OpenApiString($"{name}"));
                        }
                        else
                        {
                            schema.Enum.Add(new OpenApiString($"{name}({enumDescription}) = {Convert.ToInt64(e)}"));
                            //schema.Enum.Add(new OpenApiString($"{enumDescription}"));
                        }
                    });
                schema.Type = "string";
                schema.Format = string.Empty;
            }            
        }
    }
}
