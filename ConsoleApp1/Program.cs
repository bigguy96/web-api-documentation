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
            var apiInformation = $"{ openApiDocument.Info.Title} {openApiDocument.Info.Version}";
            var server = openApiDocument.Servers.FirstOrDefault().Url;

            var content = await File.ReadAllTextAsync(template);
            html.AppendLine(content);

            //get all endpoint infomartion.
            var paths = openApiDocument.Paths.Select(s => new Paths
            {
                Endpoint = s.Key,
                Operations = s.Value.Operations.Select(operation => new Operation
                {
                    OperationType = $"{operation.Key} - {s.Key}",
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

            var g = paths.Select(s =>s.Operations.GroupBy(gb => gb.Name));

            //var n = openApiDocument.Paths.Select(c => new { more= c.Value.Operations.Count > 1, ep = c.Key, w = c.Value.Operations }).Where(w=> w.more).ToList();

            //var t = paths.Select(s => new
            //{
            //    Endpoint = s.Operations.Select(ss => new { Endpoint = $"{ss.OperationType} - {s.Endpoint}", ss.Description, ss.Summary, ss.Name }),
            //    ContentTypes = s.Operations.Select(r => new { ContentType = r.RequestBodies.Select(rb => new { rb.ContentType }) }),
            //    Responses = s.Operations.Select(r => new { ContentType = r.Responses.Select(rb => new { rb.Name, rb.Description }) }),
            //    Parameters = s.Operations.Select(r => new { Parameters = r.Parameters.Select(pa => new { pa.Name, pa.In, pa.Type, pa.Description, pa.IsRequired, pa.Enumerations }) })
            //}).ToList();

            foreach (var item in g)
            {
                foreach (var i in item)
                {
                    var a = i.Key;
                    html.AppendLine($"<h1>{i.Key}</h1>");

                    foreach (var ii in i)
                    {
                        html.AppendLine(@"<div class=""card border-dark"">");
                        html.AppendLine($@"<div class=""card-header""><h2 class=""bg-light"">{ii.OperationType}</h2></div>");
                        html.AppendLine(@"<div class=""card-body"">");
                        html.AppendLine("<h3>Description</h3>");
                        html.AppendLine($"<p>{ii.Description}</p>");
                        html.AppendLine("<h3>Summary</h3>");
                        html.AppendLine($"<p>{ii.Summary}</p>");

                        html.AppendLine("<h4>Response Content Type</h4>");
                        html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                        html.AppendLine(@"<thead class=""thead-dark"">");
                        html.AppendLine("<tr>");
                        html.AppendLine(@"<th scope=""col"">Content Type</th>");
                        html.AppendLine(@"<th scope=""col"">Id</th>");
                        html.AppendLine(@"<th scope=""col"">Type</th>");                        
                        html.AppendLine("</tr>");
                        html.AppendLine("</thead>");
                        html.AppendLine("<tbody>");

                        foreach (var aa in ii.RequestBodies)
                        {
                            html.AppendLine("<tr>");
                            html.AppendLine($"<td>{aa.ContentType}</td>");
                            html.AppendLine($"<td>{aa.Id}</td>");
                            html.AppendLine($"<td>{aa.Type}</td>");
                            html.AppendLine("</tr>");
                        }

                        html.AppendLine("</tbody>");
                        html.AppendLine("</table>");

                        html.AppendLine("<h4>Responses</h4>");
                        html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                        html.AppendLine(@"<thead class=""thead-dark"">");
                        html.AppendLine("<tr>");
                        html.AppendLine(@"<th scope=""col"">Name</th>");
                        html.AppendLine(@"<th scope=""col"">Description</th>");                        
                        html.AppendLine("</tr>");
                        html.AppendLine("</thead>");
                        html.AppendLine("<tbody>");

                        foreach (var bb in ii.Responses)
                        {
                            html.AppendLine("<tr>");
                            html.AppendLine($"<td>{bb.Name}</td>");
                            html.AppendLine($"<td>{bb.Description}</td>");
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

                        foreach (var cc in ii.Parameters)
                        {
                            html.AppendLine("<tr>");
                            html.AppendLine($"<td>{cc.Name}</td>");
                            html.AppendLine($"<td>{cc.In}</td>");
                            html.AppendLine($"<td>{cc.Type}</td>");
                            html.AppendLine($"<td>{cc.Description}</td>");
                            html.AppendLine($"<td>{cc.IsRequired}</td>");
                            html.AppendLine($"<td>{string.Join(", ", cc.Enumerations.Select(e => e.Value))}</td>");
                            html.AppendLine("</tr>");
                        }

                        html.AppendLine("</tbody>");
                        html.AppendLine("</table>");

                        html.AppendLine("</div>");
                        html.AppendLine("</div>");
                    }

                }
            }

            //add endpoint details to html.
            //foreach (var path in paths)
            //{
            //    //html.AppendLine($"<h2>{path.Endpoint}</h2>");
            //    html.AppendLine(@"<div class=""card border-dark"">");
            //    html.AppendLine($@"<div class=""card-header""><h2 class=""bg-light"">{path.Endpoint}</h2></div>");
            //    html.AppendLine(@"<div class=""card-body"">");

            //    foreach (var operation in path.Operations)
            //    {
            //        html.AppendLine("<h3>Operations</h3>");
            //        html.AppendLine($"<p>{operation.Name}</p>");
            //        html.AppendLine($"<p>{operation.OperationType}</p>");
            //        html.AppendLine($"<p>{operation.Description}</p>");
            //        html.AppendLine($"<p>{operation.Summary}</p>");

            //        html.AppendLine("<hr />");

            //        foreach (var response in operation.Responses)
            //        {
            //            html.AppendLine("<h3>Responses</h3>");
            //            html.AppendLine($"<p>{response.Name}</p>");
            //            html.AppendLine($"<p>{response.Description}</p>");
            //        }
            //        html.AppendLine("<hr />");

            //        foreach (var parameter in operation.Parameters)
            //        {
            //            html.AppendLine("<h3>Parameters</h3>");
            //            html.AppendLine($"<p>{parameter.Name}</p>");
            //            html.AppendLine($"<p>{parameter.Type}</p>");
            //            html.AppendLine($"<p>{parameter.In}</p>");
            //            html.AppendLine($"<p>{parameter.Description}</p>");
            //            html.AppendLine($"<p>{parameter.IsRequired}</p>");

            //            if (parameter.Enumerations.Any())
            //            {
            //                html.AppendLine("<h4>Enums</h4>");
            //                html.AppendLine($"<p>{string.Join(", ", parameter.Enumerations.Select(e => e.Value))}</p>");
            //            }

            //            html.AppendLine("<hr />");
            //        }
            //        html.AppendLine("<hr />");
            //    }

            //    html.AppendLine("</div>");
            //    html.AppendLine("</div>");
            //}

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

            //add endpoint details to html.
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

            //var paths = new List<Paths>();
            //foreach (var item in openApiDocument.Paths)
            //{
            //    var path = new Paths { Endpoint = item.Key };

            //    foreach (var operation in item.Value.Operations)
            //    {
            //        if (operation.Value.RequestBody != null)
            //        {
            //            foreach (var requestBody in operation.Value.RequestBody.Content)
            //            {

            //            }
            //        }

            //        foreach (var response in operation.Value.Responses)
            //        {

            //        }

            //        foreach (var parameter in operation.Value.Parameters)
            //        {
            //            foreach (var enu in parameter.Schema.Enum)
            //            {

            //            }
            //        }
            //    }
            //    //paths.Add(path);
            //}

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}

//https://github.com/microsoft/OpenAPI.NET/blob/vnext/README.md