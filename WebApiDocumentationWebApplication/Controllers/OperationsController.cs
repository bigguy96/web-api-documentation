using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiDocumentationWebApplication.Models;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class OperationsController : BaseController
    {
        private readonly ILogger<OperationsController> logger;

        public OperationsController(ILogger<OperationsController> logger) : base(logger)
        {
            this.logger = logger;
        }
        
        public async Task<IActionResult> IndexAsync(string operation)
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var operations = openApiDocumentDetails.Paths.SelectMany(path => path.Operations.Where(o => o.Name.Equals(operation)));

            return View(new OperationsViewModel
            {
                Operations = operations,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            });
        }
    }
}
