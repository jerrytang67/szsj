using System;
using Abp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace TTWork.Abp.Core.Extensions
{
    public class FormatSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            // customize the generated schema for these types to have a format
            //if (context.ObjectType == typeof(string))
            //{
            //    return CreateSchemaWithFormat(context.ObjectType, context.Required, null, JSchemaType.String);
            //}
            //if (context.ObjectType == typeof(DateTime) || context.ObjectType == typeof(DateTime?))
            //{
            //    return CreateSchemaWithFormat(context.ObjectType, context.Required, "date-time", JSchemaType.String);
            //}

            // use default schema generation for all other types
            return null;
        }

        private JSchema CreateSchemaWithFormat(Type type, Required required, string format, JSchemaType jType)
        {
            var generator = new JSchemaGenerator();
            var schema = generator.Generate(type, required != Required.Always);
            if (!format.IsNullOrWhiteSpace())
                schema.Format = format;
            //TODO:这里为了适应delon type不支持array做的hotfix
            schema.Type = jType;
            //if (format == "date-time")
            //    schema.ExtensionData.Add("ui", new JObject { { "widget", "date" } });

            return schema;
        }
    }
}