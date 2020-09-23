using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using WebApiDocumentationLibrary.Entities;

namespace WebApiDocumentationWebApplication.Models
{
    public class BaseViewModel
    {
        public string WebApiTitle { get; internal set; }
        public string WebApiUrl { get; internal set; }
        public IEnumerable<Paths> Paths { get; set; }
        public IEnumerable<Operation> Operations { get; set; }
        public OpenApiComponents Components { get; internal set; }
        public IEnumerable<Schema> Schemas { get; internal set; }
    }
}