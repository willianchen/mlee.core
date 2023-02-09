using Microsoft.OpenApi.Models;
using mlee.Core.Library;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Filters
{

    /// <summary>
    /// 枚举架构过滤器
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (type.IsEnum)
            {
                var enumValueType = type?.GetField("value__")?.FieldType;
                var items = Enum.GetValues(type).Cast<Enum>()
                .Where(m => !m.ToString().Equals("Null")).Select(x =>
                $"{x.Description()}={Convert.ChangeType(x, enumValueType)}").ToList();

                if (items?.Count > 0)
                {
                    string description = string.Join(",", items);
                    //schema.Extensions.Add("extensions", new OpenApiObject
                    //{
                    //    ["description"] = new OpenApiString(description)
                    //});
                    //CommonUtils.GetProperyCommentBySummary

                    schema.Description = string.IsNullOrEmpty(schema.Description) ? description : $"{schema.Description}:{description}";
                }
            }
        }
    }
}
