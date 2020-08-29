using Microsoft.OpenApi.Models;

namespace ConsoleApp
{
    public class Operation
    {
        public OperationType OperationType { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Name { get; set; }
    }
}