using Microsoft.OpenApi.Models;

namespace ConsoleApp
{
    public class Parameter
    {
        public string Name { get; set; }
        public ParameterLocation? In { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; internal set; }
    }
}