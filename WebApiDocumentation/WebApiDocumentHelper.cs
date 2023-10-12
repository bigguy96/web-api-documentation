using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OpenApi.Readers;

namespace WebApiDocumentation
{
    public class WebApiDocumentHelper
    {
        private const string RequestUri = "https://wwwappstest.tc.gc.ca/Saf-Sec-Sur/13/MTAPI-TEST/swagger/docs/v1";
        public static async Task<WebApiDocumentDetails> CreateAsync()
        {
            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(RequestUri);
            var openApiDocument = new OpenApiStreamReader().Read(stream, out _);

            return new WebApiDocumentDetails(openApiDocument);
        }
    }
}