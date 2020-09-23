using System;
using System.Collections.Generic;
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
            var operations = openApiDocumentDetails.Paths.SelectMany(path => path.Operations.Where(o => o.Name.Equals(operation))).ToList();
            var vm = new BaseViewModel
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

            new WordGenerator().Generate(new ApiDetails
            {
                Operations = operations,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            });

            return View(vm);
        }

        public async Task<IActionResult> List()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var pathGroupings = openApiDocumentDetails.Paths.Select(path => path.Operations.GroupBy(operation => operation.Name));

            var vm = new BaseViewModel
            {
                Paths = openApiDocumentDetails.Paths,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            };

            return View(vm);
        }

        public async Task<IActionResult> Groups()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var pathGroupings = openApiDocumentDetails.Paths.Select(path => path.Operations.Select(x => new { x.Name })).Distinct();
            var list = new List<string>();
            var previous = string.Empty;

            foreach (var item in pathGroupings)
            {
                foreach (var item1 in item)
                {
                    var current = item1.Name;

                    if (!current.Equals(previous))
                    {
                        list.Add(item1.Name);
                    }
                    previous = current;
                }
            }

            return View(new OperationViewModel
            {
                ApiGroups = list,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            });
        }

        public async Task<IActionResult> Result(string endpoint)
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var operations = openApiDocumentDetails.Paths.SelectMany(path => path.Operations.Where(o => o.Endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase)));
            var vm = new BaseViewModel
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