using Microsoft.OpenApi.Any;
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
            var paths = openApiDocument.Paths.Select(s => new Paths
            {
                Endpoint = s.Key,
                Operations = s.Value.Operations.Select(operation => new Operation
                {
                    OperationType = operation.Key,
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

            //var paths = new List<Paths>();
            foreach (var item in openApiDocument.Paths)
            {
                var path = new Paths { Endpoint = item.Key };

                foreach (var operation in item.Value.Operations)
                {
                    if (operation.Value.RequestBody != null)
                    {
                        foreach (var requestBody in operation.Value.RequestBody.Content)
                        {
                           
                        }
                    }

                    foreach (var response in operation.Value.Responses)
                    {
                        
                    }

                    foreach (var parameter in operation.Value.Parameters)
                    {
                        foreach (var enu in parameter.Schema.Enum)
                        {
                            
                        }
                    }
                }
                //paths.Add(path);
            }

            var schemas = openApiDocument.Components.Schemas.Select(x => new Schema
            {
                Name = x.Key,
                Properties = x.Value.Properties.Select(y => new Properties
                {
                    Name = y.Key,
                    Type = $"{char.ToUpper(y.Value.Type[0])}{y.Value.Type.Substring(1)}"
                }).ToList()
            }).OrderBy(o => o.Name);

            var content = await File.ReadAllTextAsync(template);
            html.Append(content);

            foreach (var item in schemas)
            {
                html.AppendLine($"<h2>{item.Name}</h2>");
                html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                html.AppendLine(@"<thead class=""thead-dark"">");
                html.AppendLine("<tr>");
                html.AppendLine(@"<th scope=""col"">Name</th>");
                html.AppendLine(@"<th scope=""col"">Type</th>");
                html.AppendLine("</tr>");
                html.AppendLine("</thead>");
                html.AppendLine("<tbody>");

                foreach (var prop in item.Properties)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{prop.Name}</td>");
                    html.AppendLine($"<td>{prop.Type}</td>");
                    html.AppendLine("</tr>");
                }

                html.AppendLine("</tbody>");
                html.AppendLine("</table>");
            }

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            //await File.WriteAllTextAsync(Path.Combine(mydocs, "template", "schemas.html"), html.ToString());

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}

//https://github.com/microsoft/OpenAPI.NET/blob/vnext/README.md