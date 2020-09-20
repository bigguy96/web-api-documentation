using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace WebApiDocumentationLibrary.Entities
{
    public class Response
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<IDictionary<string, OpenApiSchema>> Properties { get; internal set; }
        public IEnumerable<OpenApiReference> References { get; internal set; }
    }
}