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
using ConsoleApp.Entities;

namespace ConsoleApp
{
    internal class Program
    {
        //private const string RequestUri = "https://wwwapps.tc.gc.ca/Saf-Sec-Sur/13/mtapi/swagger/docs/v1";
        //private static OpenApiDocument _openApiDocument;

        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            //var html = new StringBuilder("");
            //var sidemenu = new StringBuilder("");
            //var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var template = Path.Combine(myDocuments, "template", "docs-page.html");
            //var content = await File.ReadAllTextAsync(template);


            var openApiDocument = await OpenApiDocumentHelper.CreateAsync();
            var openApiDocumentDetails = new OpenApiDocumentDetails(openApiDocument);

            Console.WriteLine(openApiDocumentDetails.WebApiUrl);
            Console.WriteLine(openApiDocumentDetails.WebApiTitle);


            //var httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri("https://raw.githubusercontent.com/OAI/OpenAPI-Specification/")
            //};
            //var stream = await httpClient.GetStreamAsync(RequestUri);
            //_openApiDocument = new OpenApiStreamReader().Read(stream, out _);
            //var apiInformation = $"{ _openApiDocument.Info.Title} {_openApiDocument.Info.Version}";
            //var server = _openApiDocument.Servers.FirstOrDefault()?.Url;

            //html.AppendLine(content);
            //html.AppendLine($"<h1>{apiInformation}</h1>");
            //html.AppendLine($"<p><strong>{server}</strong></p>");

            ////get all endpoint information.
            //var paths = _openApiDocument.Paths.Select(path => new Paths
            //{
            //    Endpoint = path.Key,
            //    Operations = path.Value.Operations.Select(operation => new Operation
            //    {
            //        Method = operation.Key.ToString().ToLower(),
            //        OperationType = $"{path.Key}",
            //        Description = operation.Value.Description,
            //        Summary = operation.Value.Summary,
            //        Name = operation.Value.Tags[0].Name,
            //        RequestBodies = operation.Value.RequestBody != null ? operation.Value.RequestBody.Content.Select(requestBody => new RequestBody
            //        {
            //            ContentType = requestBody.Key,
            //            Id = requestBody.Value.Schema?.Reference?.Id,
            //            Type = requestBody.Value.Schema?.Type
            //        }) : new List<RequestBody>(),
            //        Responses = operation.Value.Responses.Select(response => new Response
            //        {
            //            Name = response.Key,
            //            Description = response.Value.Description
            //        }),
            //        Parameters = operation.Value.Parameters.Select(parameter => new Parameter
            //        {
            //            Name = parameter.Name,
            //            In = parameter.In,
            //            Type = parameter.Schema.Type,
            //            Description = parameter.Description,
            //            IsRequired = parameter.Required,
            //            Enumerations = parameter.Schema.Enum.Select(e => new Enums
            //            {
            //                Value = ((OpenApiPrimitive<string>)e).Value
            //            })
            //        })
            //    })
            //}).ToList();

            //var pathGroupings = paths.Select(s => s.Operations.GroupBy(gb => gb.Name));
            //var previous = string.Empty;
            //var current = string.Empty;

            //foreach (var path in pathGroupings)
            //{
            //    foreach (var group in path)
            //    {
            //        current = group.Key;

            //        if (!current.Equals(previous))
            //        {
            //            html.AppendLine($"<hr />");
            //            html.AppendLine($@"<h1 id=""{group.Key}"">{group.Key}</h1>");

            //            sidemenu.AppendLine(@"</ul>");
            //            sidemenu.AppendLine(@"<ul class=""section-items list-unstyled nav flex-column pb-3"">");
            //            sidemenu.AppendLine(@$"<li class=""nav-item section-title""><a class=""nav-link scrollto active"" href=""#{group.Key}"">{group.Key}</a></li>");
            //        }

            //        foreach (var operation in group)
            //        {
            //            var method = GetApiMethod(operation);

            //            var guid = Guid.NewGuid().ToString().Substring(0,8);
            //            html.AppendLine($@"<h2 id=""{guid}""><span class=""badge {method}"">{operation.Method.ToUpper()} - {operation.OperationType}</span></h2>");
            //            html.AppendLine("<h3>Description</h3>");
            //            html.AppendLine($"<p>{operation.Description}</p>");
            //            html.AppendLine("<h3>Summary</h3>");
            //            html.AppendLine($"<p>{operation.Summary}</p>");

            //            sidemenu.AppendLine($@"<li class=""nav-item""><a class=""nav-link scrollto"" href=""#{guid}"">{operation.Method} - {operation.OperationType}</a></li>");

            //            html.AppendLine("<h4>Response Content Type</h4>");
            //            if (operation.RequestBodies.Any())
            //            {
            //                html.AppendLine(@"<table class=""table table-bordered table-hover"">");
            //                html.AppendLine(@"<thead class=""thead-dark"">");
            //                html.AppendLine("<tr>");
            //                html.AppendLine(@"<th scope=""col"">Content Type</th>");
            //                html.AppendLine(@"<th scope=""col"">Id</th>");
            //                html.AppendLine(@"<th scope=""col"">Type</th>");
            //                html.AppendLine("</tr>");
            //                html.AppendLine("</thead>");
            //                html.AppendLine("<tbody>");

            //                var contentType = operation.RequestBodies?.SingleOrDefault(requestBody => requestBody.ContentType.Equals("application/json"));
            //                html.AppendLine("<tr>");
            //                html.AppendLine($@"<td>{contentType?.ContentType}</td>");
            //                html.AppendLine($@"<td><a href=""#{contentType?.Id}"">{contentType?.Id}</a></td>");
            //                html.AppendLine($"<td>{contentType?.Type}</td>");
            //                html.AppendLine("</tr>");
            //                html.AppendLine("</tbody>");
            //                html.AppendLine("</table>");

            //                if (contentType?.Id != null)
            //                {
            //                    var sampleJson = FormatJson(contentType.Id);

            //                    html.AppendLine("<h4>Sample Request Body</h4>");
            //                    html.AppendLine(@"<div class=""docs-code-block"">");
            //                    html.AppendLine(@"<pre class=""shadow-lg rounded""><code class=""json hljs"">");
            //                    html.AppendLine($"{sampleJson}");
            //                    html.AppendLine(@"</code></pre></div>");
            //                }

            //            }
            //            else
            //            {
            //                html.AppendLine("<p>application/json</p>");
            //            }

            //            html.AppendLine("<h4>Responses</h4>");
            //            html.AppendLine(@"<table class=""table table-bordered table-hover"">");
            //            html.AppendLine(@"<thead class=""thead-dark"">");
            //            html.AppendLine("<tr>");
            //            html.AppendLine(@"<th scope=""col"">Name</th>");
            //            html.AppendLine(@"<th scope=""col"">Description</th>");
            //            html.AppendLine("</tr>");
            //            html.AppendLine("</thead>");
            //            html.AppendLine("<tbody>");

            //            foreach (var response in operation.Responses)
            //            {
            //                html.AppendLine("<tr>");
            //                html.AppendLine($"<td>{response.Name}</td>");
            //                html.AppendLine($"<td>{response.Description}</td>");
            //                html.AppendLine("</tr>");
            //            }

            //            html.AppendLine("</tbody>");
            //            html.AppendLine("</table>");

            //            html.AppendLine("<h4>Parameters</h4>");
            //            html.AppendLine(@"<table class=""table table-bordered table-hover"">");
            //            html.AppendLine(@"<thead class=""thead-dark"">");
            //            html.AppendLine("<tr>");
            //            html.AppendLine(@"<th scope=""col"">Name</th>");
            //            html.AppendLine(@"<th scope=""col"">In</th>");
            //            html.AppendLine(@"<th scope=""col"">Type</th>");
            //            html.AppendLine(@"<th scope=""col"">Description</th>");
            //            html.AppendLine(@"<th scope=""col"">Required</th>");
            //            html.AppendLine(@"<th scope=""col"">Enums</th>");
            //            html.AppendLine("</tr>");
            //            html.AppendLine("</thead>");
            //            html.AppendLine("<tbody>");

            //            foreach (var parameter in operation.Parameters)
            //            {
            //                html.AppendLine("<tr>");
            //                html.AppendLine($"<td>{parameter.Name}</td>");
            //                html.AppendLine($"<td>{parameter.In}</td>");
            //                html.AppendLine($"<td>{parameter.Type}</td>");
            //                html.AppendLine($"<td>{parameter.Description}</td>");
            //                html.AppendLine($"<td>{parameter.IsRequired}</td>");
            //                html.AppendLine($"<td>{string.Join(", ", parameter.Enumerations.Select(e => e.Value))}</td>");
            //                html.AppendLine("</tr>");
            //            }

            //            html.AppendLine("</tbody>");
            //            html.AppendLine("</table>");
            //        }
            //    }
            //    previous = current;
            //    //sidemenu.AppendLine("</ul>");

            //}

            ////get schema information.
            //var schemas = _openApiDocument.Components.Schemas.Select(keyValuePair => new Schema
            //{
            //    Name = keyValuePair.Key,
            //    Properties = keyValuePair.Value.Properties.Select(valuePair => new Properties
            //    {
            //        Name = valuePair.Key,
            //        Type = $"{char.ToUpper(valuePair.Value.Type[0])}{valuePair.Value.Type.Substring(1)}",
            //        Enumerations = valuePair.Value.Enum.Select(e => new Enums
            //        {
            //            Value = ((OpenApiPrimitive<string>)e).Value
            //        })
            //    })
            //}).OrderBy(schema => schema.Name);

            ////add endpoint details to html.
            //foreach (var schema in schemas)
            //{
            //    html.AppendLine($@"<h2 id=""{schema.Name}"">{schema.Name}</h2>");
            //    html.AppendLine(@"<table class=""table table-bordered table-hover"">");
            //    html.AppendLine(@"<thead class=""thead-dark"">");
            //    html.AppendLine("<tr>");
            //    html.AppendLine(@"<th scope=""col"">Name</th>");
            //    html.AppendLine(@"<th scope=""col"">Type</th>");
            //    html.AppendLine("</tr>");
            //    html.AppendLine("</thead>");
            //    html.AppendLine("<tbody>");

            //    foreach (var prop in schema.Properties)
            //    {
            //        html.AppendLine("<tr>");
            //        html.AppendLine($"<td>{prop.Name}</td>");
            //        html.AppendLine($"<td>{prop.Type}</td>");
            //        html.AppendLine("</tr>");

            //        if (!prop.Enumerations.Any()) continue;

            //        html.AppendLine("<tr>");
            //        html.AppendLine($"<td>{string.Join(", ", prop.Enumerations.Select(e => e.Value))}</td>");
            //        html.AppendLine("<td>String</td>");
            //        html.AppendLine("</tr>");
            //    }

            //    html.AppendLine("</tbody>");
            //    html.AppendLine("</table>");
            //}

            ////html.AppendLine("</body>");
            ////html.AppendLine("</html>");

            //content = content.Replace("{content}", html.ToString());
            //content = content.Replace("{sidemenu}", sidemenu.ToString());
            //await File.WriteAllTextAsync(Path.Combine(myDocuments, "template", "schemas.html"), content);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        //private static StringBuilder _json;

        //private static string ConvertPropertiesToJson(KeyValuePair<string, OpenApiSchema> kvp)
        //{
        //    foreach (var (key, openApiSchema) in kvp.Value.Properties)
        //    {
        //        _json.Append(@$"   ""{key}"": ");
        //        switch (openApiSchema.Type)
        //        {
        //            case "integer":
        //                _json.Append("0,");
        //                break;

        //            case "number":
        //                _json.Append("0,");
        //                break;

        //            case "string":
        //                _json.Append(@"""string"",");
        //                break;

        //            case "boolean":
        //                _json.Append(@"true,");
        //                break;

        //            case "object":
        //                _json.Append(@" { ");
        //                var schema = _openApiDocument.Components.Schemas.SingleOrDefault(s => s.Key.Equals(openApiSchema.Reference.Id));
        //                ConvertPropertiesToJson(schema);

        //                break;

        //            case "array":
        //                if (openApiSchema.Items.Type.Equals("integer"))
        //                {
        //                    _json.Append(" [ 0 ],");
        //                }
        //                else if (openApiSchema.Items.Type.Equals("string"))
        //                {
        //                    _json.Append(@" [ ""string"" ],");
        //                }
        //                //else if (openApiSchema.Items.Type.Equals("array"))
        //                //{
        //                //    foreach (var item in openApiSchema.Items.Properties)
        //                //    {
        //                //        Json(item);
        //                //    }

        //                //}
        //                else if (openApiSchema.Items.Type.Equals("object"))
        //                {
        //                    _json.Append(" [{ ");
        //                    foreach (var item in openApiSchema.Items.Properties)
        //                    {
        //                        _json.Append(ConvertPropertyToJson(item));
        //                    }

        //                    _json.Append(" }],");
        //                }

        //                break;

        //            default:
        //                _json.Append($@"""{openApiSchema.Type}"",");
        //                break;
        //        }
        //    }

        //    _json.Remove(_json.Length - 1, 1);
        //    _json.Append("},");

        //    return _json.ToString();
        //}

        //private static string ConvertPropertyToJson(KeyValuePair<string, OpenApiSchema> kvp)
        //{
        //    var sb = new StringBuilder("");
        //    sb.Append(@$"   ""{kvp.Key}"": ");
        //    switch (kvp.Value.Type)
        //    {
        //        case "integer":
        //            sb.Append("0,");
        //            break;

        //        case "number":
        //            sb.Append("0,");
        //            break;

        //        case "string":
        //            sb.Append(@"""string"",");
        //            break;

        //        case "boolean":
        //            sb.Append(@"true,");
        //            break;

        //        case "object":
        //            sb.Append(@"{ ""__identity"": {}");
        //            sb.Append(" },");
        //            break;
        //    }

        //    return sb.ToString();
        //}

        //private static string FormatJson(string reference)
        //{
        //    _json = new StringBuilder("{");
        //    var schema = _openApiDocument.Components.Schemas.SingleOrDefault(s => s.Key.Equals(reference));
        //    var propertyValues = ConvertPropertiesToJson(schema);
        //    propertyValues = propertyValues.Remove(propertyValues.Length - 1, 1);

        //    var prettyJson = JToken.Parse(propertyValues).ToString(Formatting.Indented);

        //    return prettyJson;
        //}

        //private static string GetApiMethod(Operation operation)
        //{
        //    return operation.Method switch
        //    {
        //        "post" => "badge-post",
        //        "get" => "badge-get",
        //        "put" => "badge-put",
        //        "delete" => "badge-delete",
        //        "head" => "badge-head",
        //        _ => string.Empty
        //    };
        //}
    }
}

//https://github.com/microsoft/OpenAPI.NET/blob/vnext/README.md