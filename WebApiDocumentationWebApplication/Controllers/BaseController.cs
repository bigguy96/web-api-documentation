using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDocumentation;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<WebApiDocumentDetails> GetApiDocumentDetailsAsync()
        {
            var openApiDocumentDetails = await WebApiDocumentHelper.CreateAsync();
            
            return openApiDocumentDetails;
        }
    }
}