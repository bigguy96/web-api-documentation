using System.Collections.Generic;
using WebApiDocumentationLibrary.Entities;

namespace WebApiDocumentationWebApplication.Models
{
    public class SchemasViewModel
    {
        public string WebApiTitle { get; internal set; }
        public string WebApiUrl { get; internal set; }
        public IEnumerable<Schema> Schemas { get; internal set; }
    }
}