using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OpenApi.Readers;

namespace WebApiDocumentationLibrary
{
    public class OpenApiDocumentHelper
    {
        private const string RequestUri = "https://wwwapps.tc.gc.ca/Saf-Sec-Sur/13/mtapi/swagger/docs/v1";
        public static async Task<OpenApiDocumentDetails> CreateAsync()
        {
            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(RequestUri);
            var openApiDocument = new OpenApiStreamReader().Read(stream, out _);

            return new OpenApiDocumentDetails(openApiDocument);
        }
    }
}