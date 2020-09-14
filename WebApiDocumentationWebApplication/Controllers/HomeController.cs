using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiDocumentationLibrary;
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

        public async Task<IActionResult> Index()
        {
            var openApiDocument = await OpenApiDocumentHelper.CreateAsync();
            var openApiDocumentDetails = new OpenApiDocumentDetails(openApiDocument);
            var pathGroupings = openApiDocumentDetails.Paths.Select(path => path.Operations.GroupBy(operation => operation.Name));

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
    }
}