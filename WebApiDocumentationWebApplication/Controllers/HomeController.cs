using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp;
using ConsoleApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiDocumentationWebApplication.Models;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var openApiDocument = await OpenApiDocumentHelper.CreateAsync();
            var openApiDocumentDetails = new OpenApiDocumentDetails(openApiDocument);
            var pathGroupings = openApiDocumentDetails.Paths.Select(path => path.Operations.GroupBy(operation => operation.Name));

            ////var z = openApiDocumentDetails.Paths.ToDictionary(x => x.Operations.GroupBy(o => o.Name), x => x.Operations);
            //var g = openApiDocument.Paths.ToDictionary(x => x.Key, x => x.Value);
            //var r = openApiDocument.Paths.Select(g => g.Value.Operations.GroupBy(s =>  s.Value.Tags[0].Name));
            //var previous = string.Empty;
            //var current = string.Empty;
            //var d = new Dictionary<string, IEnumerable<Operation>>();
            //foreach (var item in openApiDocument.Paths)
            //{
            //    foreach (var o in item.Value.Operations)
            //    {
            //        //current = o.Name;

            //        if (!current.Equals(previous))
            //        {
            //        }
            //    }
            //    previous = current;
            //}

            var vm = new HomeViewModel
            {
                Paths = openApiDocumentDetails.Paths,
                Groupings = pathGroupings,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
