using System;

namespace WebApiDocumentationLibrary
{
    internal class Program
    {
        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            var openApiDocument = await OpenApiDocumentHelper.CreateAsync();
            var openApiDocumentDetails = new OpenApiDocumentDetails(openApiDocument);

            Console.WriteLine(openApiDocumentDetails.WebApiUrl);
            Console.WriteLine(openApiDocumentDetails.WebApiTitle);

            var generator = new HtmlGenerator(openApiDocumentDetails);
            await generator.AddPathsAsync();
           
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
    }
}

//https://github.com/microsoft/OpenAPI.NET/blob/vnext/README.md