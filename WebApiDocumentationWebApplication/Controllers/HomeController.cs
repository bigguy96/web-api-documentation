using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiDocumentationLibrary;
using WebApiDocumentationWebApplication.Models;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {            
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var pathGroupings = openApiDocumentDetails.Paths.Select(path => path.Operations.GroupBy(operation => operation.Name));

            var vm = new HomeViewModel
            {
                Paths = openApiDocumentDetails.Paths,               
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            };

            return View(vm);
        }
        
        public async Task<IActionResult> Operation()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var pathGroupings = openApiDocumentDetails.Paths.Select(path => path.Operations.Select(x=> new { x.Name })).Distinct();
            var list = new List<string>();
            var current = string.Empty;
            var previous = string.Empty;

            foreach (var item in pathGroupings)
            {
                foreach (var item1 in item)
                {
                    current = item1.Name;

                    if (!current.Equals(previous))
                    {
                        list.Add(item1.Name);
                    } 
                    previous = current;
                }                
            }

            return View(new OperationViewModel { Operations = list });
        }
    }
}