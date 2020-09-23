using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDocumentationWebApplication.Models;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();
            var vm = new BaseViewModel
            {
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            };

            return View(vm);
        }
    }
}