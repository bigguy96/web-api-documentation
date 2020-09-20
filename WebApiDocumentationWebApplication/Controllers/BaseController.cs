using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDocumentationLibrary;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<OpenApiDocumentDetails> GetApiDocumentDetailsAsync()
        {
            var openApiDocument = await OpenApiDocumentHelper.CreateAsync();
            var openApiDocumentDetails = new OpenApiDocumentDetails(openApiDocument);

            return openApiDocumentDetails;
        }
    }
}