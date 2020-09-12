using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiDocumentationLibrary.Extensions
{
    public static class OpenApiDocumentExtensions
    {
        private static StringBuilder _json;
        private static OpenApiComponents _openApiComponents;

        public static string FormatJson(this string reference, OpenApiComponents components)
        {
            _json = new StringBuilder("{");
            _openApiComponents = components;

            var schema = _openApiComponents.Schemas.SingleOrDefault(s => s.Key.Equals(reference));
            var propertyValues = ConvertPropertiesToJson(schema);
            propertyValues = propertyValues.Remove(propertyValues.Length - 1, 1);

            var prettyJson = JToken.Parse(propertyValues).ToString(Formatting.Indented);

            return prettyJson;
        }

        private static string ConvertPropertiesToJson(KeyValuePair<string, OpenApiSchema> kvp)
        {
            foreach (var (key, openApiSchema) in kvp.Value.Properties)
            {
                _json.Append(@$"   ""{key}"": ");
                switch (openApiSchema.Type)
                {
                    case "integer":
                        _json.Append("0,");
                        break;

                    case "number":
                        _json.Append("0,");
                        break;

                    case "string":
                        _json.Append(@"""string"",");
                        break;

                    case "boolean":
                        _json.Append(@"true,");
                        break;

                    case "object":
                        _json.Append(@" { ");
                        var schema = _openApiComponents.Schemas.SingleOrDefault(s => s.Key.Equals(openApiSchema.Reference.Id));
                        ConvertPropertiesToJson(schema);

                        break;

                    case "array":
                        if (openApiSchema.Items.Type.Equals("integer"))
                        {
                            _json.Append(" [ 0 ],");
                        }

                        else if (openApiSchema.Items.Type.Equals("string"))
                        {
                            _json.Append(@" [ ""string"" ],");
                        } 
                        
                        else if (openApiSchema.Items.Type.Equals("object"))
                        {
                            _json.Append(" [{ ");
                            foreach (var item in openApiSchema.Items.Properties)
                            {
                                _json.Append(ConvertPropertyToJson(item));
                            }

                            _json.Append(" }],");
                        }

                        break;

                    default:
                        _json.Append($@"""{openApiSchema.Type}"",");
                        break;
                }
            }

            _json.Remove(_json.Length - 1, 1);
            _json.Append("},");

            return _json.ToString();
        }

        private static string ConvertPropertyToJson(KeyValuePair<string, OpenApiSchema> kvp)
        {
            var sb = new StringBuilder("");
            sb.Append(@$"   ""{kvp.Key}"": ");
            switch (kvp.Value.Type)
            {
                case "integer":
                    sb.Append("0,");
                    break;

                case "number":
                    sb.Append("0,");
                    break;

                case "string":
                    sb.Append(@"""string"",");
                    break;

                case "boolean":
                    sb.Append(@"true,");
                    break;

                case "object":
                    sb.Append(@"{ ""__identity"": {}");
                    sb.Append(" },");
                    break;
            }

            return sb.ToString();
        }
    }
}