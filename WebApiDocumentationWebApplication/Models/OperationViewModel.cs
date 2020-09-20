using System.Collections.Generic;

namespace WebApiDocumentationWebApplication.Models
{
    public class OperationViewModel
    {
        public IEnumerable<string> Operations { get; set; }
        public string WebApiTitle { get; internal set; }
        public string WebApiUrl { get; internal set; }
    }
}