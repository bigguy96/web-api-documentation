using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class Operation
    {
        public string OperationType { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Name { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; }
        public IEnumerable<Response> Responses { get; set; }
        public IEnumerable<RequestBody> RequestBodies { get; set; }        
    }
}