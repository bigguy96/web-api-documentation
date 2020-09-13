using System.Collections.Generic;

namespace WebApiDocumentationLibrary.Entities
{
    public class Paths
    {
        public string Endpoint { get; set; }
        public IEnumerable<Operation> Operations { get; set; }        
    }
}