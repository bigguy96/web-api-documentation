using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using WebApiDocumentation.Entities;

namespace WordDocumentGenerator
{
    public class ApiDetails
    {
        public string WebApiTitle { get; set; }
        public string WebApiUrl { get; set; }
        public IEnumerable<Operation> Operations { get; set; }
        public OpenApiComponents Components { get; set; }
    }
}