using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        private static OpenApiDocument _openApiDocument;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var html = new StringBuilder("");
            var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var template = Path.Combine(mydocs, "template", "template.html");

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://raw.githubusercontent.com/OAI/OpenAPI-Specification/")
            };
            var stream = await httpClient.GetStreamAsync("https://wwwapps.tc.gc.ca/Saf-Sec-Sur/13/mtapi/swagger/docs/v1");
            var openApiDocument = new OpenApiStreamReader().Read(stream, out var diagnostic);
            var apiInformation = $"{ openApiDocument.Info.Title} {openApiDocument.Info.Version}";
            var server = openApiDocument.Servers.FirstOrDefault().Url;

            _openApiDocument = openApiDocument;

            var content = await File.ReadAllTextAsync(template);
            html.AppendLine(content);

            //get all endpoint infomartion.
            var paths = openApiDocument.Paths.Select(s => new Paths
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
                            html.AppendLine($"<td>{contentType?.ContentType}</td>");
                            html.AppendLine($"<td>{contentType?.Id}</td>");
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
                        html.AppendLine(@"<th scope=""col"">IsRequired</th>");
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
            var schemas = openApiDocument.Components.Schemas.Select(x => new Schema
            {
                Name = x.Key,
                Properties = x.Value.Properties.Select(y => new Properties
                {
                    Name = y.Key,
                    Type = $"{char.ToUpper(y.Value.Type[0])}{y.Value.Type.Substring(1)}",
                    Enumerations = y.Value.Enum.Select(e => new Enums
                    {
                        Value = ((OpenApiPrimitive<string>)e).Value
                    })
                })
            }).OrderBy(o => o.Name);

            var f = openApiDocument.Components.Schemas.SingleOrDefault(s => s.Key.Equals("UserRegistrationContext"));
            var ds = GetProperties(f);

            //add endpoint details to html.
            foreach (var schema in schemas)
            {
                html.AppendLine($"<h2>{schema.Name}</h2>");
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

                    if (prop.Enumerations.Any())
                    {
                        html.AppendLine("<tr>");
                        html.AppendLine($"<td>{string.Join(", ", prop.Enumerations.Select(e => e.Value))}</td>");
                        html.AppendLine("<td>String</td>");
                        html.AppendLine("</tr>");
                    }
                }

                html.AppendLine("</tbody>");
                html.AppendLine("</table>");
            }

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            await File.WriteAllTextAsync(Path.Combine(mydocs, "template", "schemas.html"), html.ToString());

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static List<string> _properties = new List<string>();
        private static IEnumerable<string> GetProperties(KeyValuePair<string, OpenApiSchema> kvp)
        {
            foreach (var prop in kvp.Value.Properties)
            {
                _properties.Add($"{prop.Key};{prop.Value.Type}");

                if (prop.Value.Reference != null)
                {
                    var schema = _openApiDocument.Components.Schemas.SingleOrDefault(s => s.Key.Equals(prop.Value.Reference.Id));
                    GetProperties(schema);
                }
            }

            return _properties;
        }
    }
}

//https://github.com/microsoft/OpenAPI.NET/blob/vnext/README.md