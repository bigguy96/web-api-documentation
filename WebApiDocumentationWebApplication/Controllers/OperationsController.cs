using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDocumentationWebApplication.Models;
using WebApiDocumentationWebApplication.Utilities;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class OperationsController : BaseController
    {
        private readonly IViewRenderService _viewRenderService;

        public OperationsController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public async Task<IActionResult> IndexAsync(string operation)
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
            var result = await _viewRenderService.RenderToStringAsync("Operations/Index", vm);

            return View(vm);
        }
    }
}