﻿using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class Parameter
    {
        public string Name { get; set; }
        public ParameterLocation? In { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; internal set; }
        public IEnumerable<Enums> Enumerations { get; set; }
    }
}