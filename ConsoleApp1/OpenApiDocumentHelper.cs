using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace WebApiDocumentationLibrary
{
    public class OpenApiDocumentHelper
    {
        private const string RequestUri = "https://wwwapps.tc.gc.ca/Saf-Sec-Sur/13/mtapi/swagger/docs/v1";
        public static async Task<OpenApiDocument> CreateAsync()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://raw.githubusercontent.com/OAI/OpenAPI-Specification/")
            };
            var stream = await httpClient.GetStreamAsync(RequestUri);

            return new OpenApiStreamReader().Read(stream, out _);
        }
    }
}