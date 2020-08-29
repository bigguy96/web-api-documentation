using System.Collections.Generic;

namespace ConsoleApp
{
    public class Schema
    {
        public Schema()
        {
            Properties = new List<Properties>();
        }

        public string Name { get; set; }
        public IList<Properties> Properties { get; set; }
    }

    public class Properties
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
