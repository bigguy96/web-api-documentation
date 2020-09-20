using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using WebApiDocumentationLibrary.Entities;

namespace WebApiDocumentationWebApplication.Models
{
    public class ReportsViewModel
    {
        public IEnumerable<Operation> Operations { get; set; }
        public string WebApiTitle { get; internal set; }
        public string WebApiUrl { get; internal set; }
        public OpenApiComponents Components { get; internal set; }
    }
}
