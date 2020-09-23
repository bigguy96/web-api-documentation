using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using WebApiDocumentationLibrary.Entities;

namespace WebApiDocumentationLibrary
{
    public class OpenApiDocumentDetails
    {
        private readonly OpenApiDocument _openApiDocument;

        public string WebApiTitle { get; private set; }
        public string WebApiUrl { get; private set; }
        public IEnumerable<Paths> Paths { get; private set; }
        public OpenApiComponents Components { get; private set; }
        public IEnumerable<Schema> Schemas { get; private set; }

        public OpenApiDocumentDetails(OpenApiDocument openApiDocument)
        {
            _openApiDocument = openApiDocument;

            GetWebApiDetails();
            GetPathDetails();
            GetComponentsDetails();
            GetSchemaDetails();
        }

        private void GetComponentsDetails()
        {
            Components = _openApiDocument.Components;
        }

        private void GetWebApiDetails()
        {
            WebApiTitle = $"{ _openApiDocument.Info.Title} {_openApiDocument.Info.Version}";
            WebApiUrl = _openApiDocument.Servers.FirstOrDefault()?.Url;
        }

        private void GetPathDetails()
        {
            Paths = _openApiDocument.Paths.Select(path => new Paths
            {
                Endpoint = path.Key,
                Operations = path.Value.Operations.Select(operation => new Operation
                {
                    Method = operation.Key.ToString().ToLower(),
                    Endpoint = $"{path.Key}",
                    Description = operation.Value.Description,
                    Summary = operation.Value.Summary,
                    Name = operation.Value.Tags[0].Name,
                    RequestBodies = operation.Value.RequestBody != null
                        ? operation.Value.RequestBody.Content.Select(requestBody => new RequestBody
                        {
                            ContentType = requestBody.Key,
                            Id = requestBody.Value.Schema?.Reference?.Id,
                            Type = requestBody.Value.Schema?.Type
                        })
                        : new List<RequestBody>(),
                    Responses = operation.Value.Responses.Select(response => new Response
                    {
                        Name = response.Key,
                        Description = response.Value.Description,
                        Properties = response.Value.Content.Select(kvp=> kvp.Value.Schema.Properties),
                        References = response.Value.Content.Select(kvp => kvp.Value.Schema.Reference)
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

        private void GetSchemaDetails()
        {
            Schemas = _openApiDocument.Components.Schemas.Select(keyValuePair => new Schema
            {
                Name = keyValuePair.Key,
                Properties = keyValuePair.Value.Properties.Select(valuePair => new Properties
                {
                    Name = valuePair.Key,
                    Type = $"{char.ToUpper(valuePair.Value.Type[0])}{valuePair.Value.Type.Substring(1)}",
                    Enumerations = valuePair.Value.Enum.Select(e => new Enums
                    {
                        Value = ((OpenApiPrimitive<string>)e).Value
                    })
                })
            }).ToList();
        }
    }
}

//https://github.com/microsoft/OpenAPI.NET/blob/vnext/README.md