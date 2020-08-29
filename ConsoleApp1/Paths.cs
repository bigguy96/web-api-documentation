using System.Collections.Generic;

namespace ConsoleApp
{
    public class Paths
    {
        public Paths()
        {
            Parameters = new List<Parameter>();
            Responses = new List<Response>();
            Operations = new List<Operation>();
            RequestBodies = new List<RequestBody>();
            Enumerations = new List<Enums>();
        }
        
        public string Endpoint { get; set; }       
        public IList<Parameter> Parameters { get; set; }
        public IList<Response> Responses { get; set; }
        public IList<Operation> Operations { get; set; }
        public IList<RequestBody> RequestBodies { get; set; }
        public IList<Enums> Enumerations { get; set; }
    }
}