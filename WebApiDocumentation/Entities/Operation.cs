using System.Collections.Generic;

namespace WebApiDocumentation.Entities
{
    public class Operation
    {
        public string Endpoint { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Name { get; set; }
        public string Method { get; set; }
        public string FullPath
        {
            get
            {
                return $"{Method.ToUpper()} - {Endpoint}";
            }
        }
        public IEnumerable<Parameter> Parameters { get; set; }
        public IEnumerable<Response> Responses { get; set; }
        public IEnumerable<RequestBody> RequestBodies { get; set; }
    }
}