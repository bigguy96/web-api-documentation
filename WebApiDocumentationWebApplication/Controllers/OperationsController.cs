using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDocumentationWebApplication.Models;
using WebApiDocumentationWebApplication.Utilities;
using WordDocumentGenerator;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class OperationsController : BaseController
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly IEmailService _emailService;

        public OperationsController(IViewRenderService viewRenderService, IEmailService emailService)
        {
            _viewRenderService = viewRenderService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(string operation)
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var operations = openApiDocumentDetails.Paths.SelectMany(path => path.Operations.Where(o => o.Name.Equals(operation)));
            var vm = new OperationsViewModel
            {
                Operations = operations,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            };

            //var result = await _viewRenderService.RenderToStringAsync("Operations/Index", vm);
            //var content = await System.IO.File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"template", "output.html"));

            //content = content.Replace("{content}", result);
            //await _emailService.SendAsync("noreply@fakemail.com", "test", "test", content);

            //new WordGenerator().Generate(content);

            return View(vm);
        }

        public async Task<IActionResult> Result(string endpoint)
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var operations = openApiDocumentDetails.Paths.SelectMany(path => path.Operations.Where(o => o.Endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase)));
            var vm = new OperationsViewModel
            {
                Operations = operations,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            };

            return View(vm);
        }

        public async Task<IActionResult> Search()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var vm = new SearchViewModel { WebApiTitle = openApiDocumentDetails.WebApiTitle, WebApiUrl = openApiDocumentDetails.WebApiUrl };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel searchViewModel)
        {
            return RedirectToAction("Result", new {endpoint = searchViewModel.Endpoint});
        }
    }
}