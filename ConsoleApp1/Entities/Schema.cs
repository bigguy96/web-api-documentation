using System.Collections.Generic;

namespace WebApiDocumentationLibrary.Entities
{
    public class Schema
    {
        public string Name { get; set; }
        public IEnumerable<Properties> Properties { get; set; }
    }

    public class Properties
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public IEnumerable<Enums> Enumerations { get; set; }
    }
}