using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDocumentationWebApplication.Models;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class ReportsController : BaseController
    {
        public ReportsController()
        {

        }

        public async Task<IActionResult> Description()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var operations = openApiDocumentDetails.Paths.SelectMany(path => path.Operations.Where(o => string.IsNullOrWhiteSpace(o.Description)));

            return View(new ReportsViewModel
            {
                Operations = operations,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            });
        }

        public async Task<IActionResult> Summary()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var operations = openApiDocumentDetails.Paths.SelectMany(path => path.Operations.Where(o => string.IsNullOrWhiteSpace(o.Summary)));

            return View(new ReportsViewModel
            {
                Operations = operations,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            });
        }

        public async Task<IActionResult> Parameters()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var operations = openApiDocumentDetails.Paths
                .SelectMany(path => path.Operations.Where(o => o.Parameters.Any(w => string.IsNullOrWhiteSpace(w.Description) && !w.Name.Equals("app-jwt"))));

            return View(new ReportsViewModel
            {
                Operations = operations,
                Components = openApiDocumentDetails.Components,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            });
        }
    }
}