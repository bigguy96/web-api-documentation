using System.Collections.Generic;

namespace ConsoleApp
{
    public class Paths
    {
        //public Paths()
        //{
        //    //Parameters = new List<Parameter>();
        //    //Responses = new List<Response>();
        //    Operations = new List<Operation>();
        //    //RequestBodies = new List<RequestBody>();
        //    //Enumerations = new List<Enums>();
        //}
        
        public string Endpoint { get; set; }
        public IEnumerable<Operation> Operations { get; set; }        
    }
}