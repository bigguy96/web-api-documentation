using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDocumentationWebApplication.Models;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class SchemasController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var openApiDocumentDetails = await GetApiDocumentDetailsAsync();            
            var vm = new BaseViewModel
            {
                Schemas = openApiDocumentDetails.Schemas,
                WebApiTitle = openApiDocumentDetails.WebApiTitle,
                WebApiUrl = openApiDocumentDetails.WebApiUrl
            };            

            return View(vm);
        }        
    }
}