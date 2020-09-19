using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiDocumentationLibrary;

namespace WebApiDocumentationWebApplication.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }
        
        protected async Task<OpenApiDocumentDetails> GetApiDocumentDetailsAsync()
        {
            var openApiDocument = await OpenApiDocumentHelper.CreateAsync();
            var openApiDocumentDetails = new OpenApiDocumentDetails(openApiDocument);

            return openApiDocumentDetails;
        }
    }
}