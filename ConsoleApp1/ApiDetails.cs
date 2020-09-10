using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ApiDetails
    {
        private const string RequestUri = "https://wwwapps.tc.gc.ca/Saf-Sec-Sur/13/mtapi/swagger/docs/v1";
        private OpenApiDocument _openApiDocument;

        public ApiDetails()
        {

        }



        public async Task GetApiDetailsAsync()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://raw.githubusercontent.com/OAI/OpenAPI-Specification/") };
            var stream = await httpClient.GetStreamAsync(RequestUri);
            _openApiDocument = new OpenApiStreamReader().Read(stream, out _);

            //get all endpoint information.
            var paths = _openApiDocument.Paths.Select(path => new Paths
            {
                Endpoint = path.Key,
                Operations = path.Value.Operations.Select(operation => new Operation
                {
                    Method = operation.Key.ToString().ToLower(),
                    OperationType = $"{path.Key}",
                    Description = operation.Value.Description,
                    Summary = operation.Value.Summary,
                    Name = operation.Value.Tags[0].Name,
                    RequestBodies = operation.Value.RequestBody != null ? operation.Value.RequestBody.Content.Select(requestBody => new RequestBody
                    {
                        ContentType = requestBody.Key,
                        Id = requestBody.Value.Schema?.Reference?.Id,
                        Type = requestBody.Value.Schema?.Type
                    }) : new List<RequestBody>(),
                    Responses = operation.Value.Responses.Select(response => new Response
                    {
                        Name = response.Key,
                        Description = response.Value.Description
                    }),
                    Parameters = operation.Value.Parameters.Select(parameter => new Parameter
                    {
                        Name = parameter.Name,
                        In = parameter.In,
                        Type = parameter.Schema.Type,
                        Description = parameter.Description,
                        IsRequired = parameter.Required,
                        Enumerations = parameter.Schema.Enum.Select(e => new Enums
                        {
                            Value = ((OpenApiPrimitive<string>)e).Value
                        })
                    })
                })
            }).ToList();
        }
    }
}
