using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ConsoleApp
{
    internal class Program
    {
        private const string RequestUri = "https://wwwapps.tc.gc.ca/Saf-Sec-Sur/13/mtapi/swagger/docs/v1";
        private static OpenApiDocument _openApiDocument;
        private static readonly List<string> Properties = new List<string>();

        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            var html = new StringBuilder("");
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var template = Path.Combine(myDocuments, "template", "template.html");
            var content = await File.ReadAllTextAsync(template);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://raw.githubusercontent.com/OAI/OpenAPI-Specification/")
            };
            var stream = await httpClient.GetStreamAsync(RequestUri);
            _openApiDocument = new OpenApiStreamReader().Read(stream, out _);
            var apiInformation = $"{ _openApiDocument.Info.Title} {_openApiDocument.Info.Version}";
            var server = _openApiDocument.Servers.FirstOrDefault()?.Url;


            html.AppendLine(content);
            html.AppendLine($"<h1>{apiInformation}</h1>");
            html.AppendLine($"<p><strong>{server}</strong></p>");

            //get all endpoint information.
            var paths = _openApiDocument.Paths.Select(s => new Paths
            {
                Endpoint = s.Key,
                Operations = s.Value.Operations.Select(operation => new Operation
                {
                    OperationType = $"{operation.Key.ToString().ToUpper()} - {s.Key}",
                    Description = operation.Value.Description,
                    Summary = operation.Value.Summary,
                    Name = operation.Value.Tags[0].Name,
                    RequestBodies = operation.Value.RequestBody != null ? operation.Value.RequestBody.Content.Select(c => new RequestBody
                    {
                        ContentType = c.Key,
                        Id = c.Value.Schema?.Reference?.Id,
                        Type = c.Value.Schema?.Type
                    }) : new List<RequestBody>(),
                    Responses = operation.Value.Responses.Select(r => new Response
                    {
                        Name = r.Key,
                        Description = r.Value.Description
                    }),
                    Parameters = operation.Value.Parameters.Select(p => new Parameter
                    {
                        Name = p.Name,
                        In = p.In,
                        Type = p.Schema.Type,
                        Description = p.Description,
                        IsRequired = p.Required,
                        Enumerations = p.Schema.Enum.Select(e => new Enums
                        {
                            Value = ((OpenApiPrimitive<string>)e).Value
                        })
                    })
                })
            }).ToList();

            var pathGroupings = paths.Select(s => s.Operations.GroupBy(gb => gb.Name));

            foreach (var path in pathGroupings)
            {
                foreach (var group in path)
                {
                    html.AppendLine($"<h1>{group.Key}</h1>");

                    foreach (var operation in group)
                    {
                        html.AppendLine(@"<div class=""card border-dark"">");
                        html.AppendLine($@"<div class=""card-header""><h2 class=""bg-light"">{operation.OperationType}</h2></div>");
                        html.AppendLine(@"<div class=""card-body"">");
                        html.AppendLine("<h3>Description</h3>");
                        html.AppendLine($"<p>{operation.Description}</p>");
                        html.AppendLine("<h3>Summary</h3>");
                        html.AppendLine($"<p>{operation.Summary}</p>");

                        html.AppendLine("<h4>Response Content Type</h4>");
                        if (operation.RequestBodies.Any())
                        {
                            html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                            html.AppendLine(@"<thead class=""thead-dark"">");
                            html.AppendLine("<tr>");
                            html.AppendLine(@"<th scope=""col"">Content Type</th>");
                            html.AppendLine(@"<th scope=""col"">Id</th>");
                            html.AppendLine(@"<th scope=""col"">Type</th>");
                            html.AppendLine("</tr>");
                            html.AppendLine("</thead>");
                            html.AppendLine("<tbody>");

                            var contentType = operation.RequestBodies?.SingleOrDefault(requestBody => requestBody.ContentType.Equals("application/json"));
                            html.AppendLine("<tr>");
                            html.AppendLine($@"<td>{contentType?.ContentType}</td>");
                            html.AppendLine($@"<td><a href=""#{contentType?.Id}"">{contentType?.Id}</a></td>");
                            html.AppendLine($"<td>{contentType?.Type}</td>");
                            html.AppendLine("</tr>");
                            html.AppendLine("</tbody>");
                            html.AppendLine("</table>");
                        }
                        else
                        {
                            html.AppendLine("<p>application/json</p>");
                        }

                        html.AppendLine("<h4>Responses</h4>");
                        html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                        html.AppendLine(@"<thead class=""thead-dark"">");
                        html.AppendLine("<tr>");
                        html.AppendLine(@"<th scope=""col"">Name</th>");
                        html.AppendLine(@"<th scope=""col"">Description</th>");
                        html.AppendLine("</tr>");
                        html.AppendLine("</thead>");
                        html.AppendLine("<tbody>");

                        foreach (var response in operation.Responses)
                        {
                            html.AppendLine("<tr>");
                            html.AppendLine($"<td>{response.Name}</td>");
                            html.AppendLine($"<td>{response.Description}</td>");
                            html.AppendLine("</tr>");
                        }

                        html.AppendLine("</tbody>");
                        html.AppendLine("</table>");

                        html.AppendLine("<h4>Parameters</h4>");
                        html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                        html.AppendLine(@"<thead class=""thead-dark"">");
                        html.AppendLine("<tr>");
                        html.AppendLine(@"<th scope=""col"">Name</th>");
                        html.AppendLine(@"<th scope=""col"">In</th>");
                        html.AppendLine(@"<th scope=""col"">Type</th>");
                        html.AppendLine(@"<th scope=""col"">Description</th>");
                        html.AppendLine(@"<th scope=""col"">Is Required</th>");
                        html.AppendLine(@"<th scope=""col"">Enums</th>");
                        html.AppendLine("</tr>");
                        html.AppendLine("</thead>");
                        html.AppendLine("<tbody>");

                        foreach (var parameter in operation.Parameters)
                        {
                            html.AppendLine("<tr>");
                            html.AppendLine($"<td>{parameter.Name}</td>");
                            html.AppendLine($"<td>{parameter.In}</td>");
                            html.AppendLine($"<td>{parameter.Type}</td>");
                            html.AppendLine($"<td>{parameter.Description}</td>");
                            html.AppendLine($"<td>{parameter.IsRequired}</td>");
                            html.AppendLine($"<td>{string.Join(", ", parameter.Enumerations.Select(e => e.Value))}</td>");
                            html.AppendLine("</tr>");
                        }

                        html.AppendLine("</tbody>");
                        html.AppendLine("</table>");

                        html.AppendLine("</div>");
                        html.AppendLine("</div>");
                    }

                }
            }

            //get schema information.
            var schemas = _openApiDocument.Components.Schemas.Select(keyValuePair => new Schema
            {
                Name = keyValuePair.Key,
                Properties = keyValuePair.Value.Properties.Select(valuePair => new Properties
                {
                    Name = valuePair.Key,
                    Type = $"{char.ToUpper(valuePair.Value.Type[0])}{valuePair.Value.Type.Substring(1)}",
                    Enumerations = valuePair.Value.Enum.Select(e => new Enums
                    {
                        Value = ((OpenApiPrimitive<string>)e).Value
                    })
                })
            }).OrderBy(schema => schema.Name);

            var f = _openApiDocument.Components.Schemas.SingleOrDefault(s => s.Key.Equals("UserActivationContext"));
            var ds = GetProperties(f);
            var jj = Json(f);

            jj = jj.Remove(jj.Length - 1, 1);

            string prettyJson = JToken.Parse(jj).ToString(Formatting.Indented);

            var sb = new StringBuilder("");
            sb.AppendLine("{");
            foreach (var item in ds)
            {
                var split = item.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var prop = split[0];
                var type = split[1];

                sb.Append(@$"   ""{prop}"": ");
                switch (type)
                {
                    case "integer":
                        sb.AppendLine("0,");
                        break;

                    case "string":
                        sb.AppendLine(@"""string"",");
                        break;

                    case "object":
                        sb.AppendLine(@" { ");
                        break;

                    case "array":
                        sb.AppendLine($" [ {(split[2].Equals("integer") ? "0" : @"""string""")} ],");
                        break;

                    default:
                        break;
                }
            }
            sb.Remove(sb.Length - 3, 1);
            sb.Append("}");
            var json = sb.ToString();

            //add endpoint details to html.
            foreach (var schema in schemas)
            {
                html.AppendLine($@"<h2 id=""{schema.Name}"">{schema.Name}</h2>");
                html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                html.AppendLine(@"<thead class=""thead-dark"">");
                html.AppendLine("<tr>");
                html.AppendLine(@"<th scope=""col"">Name</th>");
                html.AppendLine(@"<th scope=""col"">Type</th>");
                html.AppendLine("</tr>");
                html.AppendLine("</thead>");
                html.AppendLine("<tbody>");

                foreach (var prop in schema.Properties)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{prop.Name}</td>");
                    html.AppendLine($"<td>{prop.Type}</td>");
                    html.AppendLine("</tr>");

                    if (!prop.Enumerations.Any()) continue;

                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{string.Join(", ", prop.Enumerations.Select(e => e.Value))}</td>");
                    html.AppendLine("<td>String</td>");
                    html.AppendLine("</tr>");
                }

                html.AppendLine("</tbody>");
                html.AppendLine("</table>");
            }

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            await File.WriteAllTextAsync(Path.Combine(myDocuments, "template", "schemas.html"), html.ToString());

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static IEnumerable<string> GetProperties(KeyValuePair<string, OpenApiSchema> kvp)
        {
            foreach (var (key, openApiSchema) in kvp.Value.Properties)
            {
                Properties.Add($"{key};{openApiSchema.Type};{openApiSchema.Items?.Type ?? ""}");

                if (openApiSchema.Reference == null) continue;

                var schema = _openApiDocument.Components.Schemas.SingleOrDefault(s => s.Key.Equals(openApiSchema.Reference.Id));
                GetProperties(schema);
            }

            return Properties;
        }

        private static StringBuilder json = new StringBuilder("{");
        private static string Json(KeyValuePair<string, OpenApiSchema> kvp)
        {
            //var json = new StringBuilder("");
            //json.AppendLine("{");
            foreach (var (key, openApiSchema) in kvp.Value.Properties)
            {
                //Properties.Add($"{key};{openApiSchema.Type};{openApiSchema.Items?.Type ?? ""}");

                json.Append(@$"   ""{key}"": ");
                switch (openApiSchema.Type)
                {
                    case "integer":
                        json.Append("0,");
                        break;

                    case "string":
                        json.Append(@"""string"",");
                        break;

                    case "object":
                        json.Append(@" { ");
                        var schema = _openApiDocument.Components.Schemas.SingleOrDefault(s => s.Key.Equals(openApiSchema.Reference.Id));
                        Json(schema);

                        break;

                    case "array":
                        json.Append($" [ {(openApiSchema.Items.Type.Equals("integer") ? "0" : @"""string""")} ],");
                        break;

                    default:
                        break;
                }
            }

            json.Remove(json.Length - 1, 1);
            json.Append("},");

            return json.ToString();
        }
    }
}

//https://github.com/microsoft/OpenAPI.NET/blob/vnext/README.md