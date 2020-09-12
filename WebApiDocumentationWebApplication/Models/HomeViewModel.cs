using ConsoleApp;
using ConsoleApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDocumentationWebApplication.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Paths> Paths { get; set; }
        public string WebApiTitle { get; internal set; }
        public string WebApiUrl { get; internal set; }
        public IEnumerable<IEnumerable<IGrouping<string, Operation>>> Groupings { get; internal set; }
    }
}
