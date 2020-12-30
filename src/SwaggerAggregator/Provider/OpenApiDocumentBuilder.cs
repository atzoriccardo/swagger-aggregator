using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAggregator
{
    public class OpenApiDocumentBuilder
    {
        private readonly OpenApiDocument _document;

        public OpenApiDocumentBuilder()
        {
            _document = new OpenApiDocument
            {
                Servers = new List<OpenApiServer>(),
                Paths = new OpenApiPaths(),
                Components = new OpenApiComponents()
            };
        }

        public OpenApiDocumentBuilder SetInfo(Info info, string version)
        {
            _document.Info = new OpenApiInfo
            {
                Title = info.Title,
                Description = info.Description,
                Version = version ?? "1.0.0"
            };
            return this;
        }

        public OpenApiDocumentBuilder SetServer(List<Server> servers)
        {
            foreach (var item in servers)
            {
                _document.Servers.Add(new OpenApiServer
                {
                    Url = item.Url,
                    Description = item.Description
                });
            }
            return this;
        }

        public OpenApiDocumentBuilder SetPath(OpenApiPaths currentServicePath, bool removeApiPrefix = false)
        {
            foreach (var item in currentServicePath)
            {
                if (removeApiPrefix)
                {
                    var path = item.Key.Replace("/api", "");
                    _document.Paths[path] = item.Value;
                }
                else
                {
                    _document.Paths[item.Key] = item.Value;
                }
            }

            return this;
        }

        public OpenApiDocumentBuilder SetSchema(IDictionary<string, OpenApiSchema> schemas)
        {
            foreach (var schema in schemas)
            {
                _document.Components.Schemas.Add(schema.Key, schema.Value);
            }

            return this;
        }

        public OpenApiDocument Build()
        {
            return _document;
        }
    }
}
