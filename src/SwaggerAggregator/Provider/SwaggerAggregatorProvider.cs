using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SwaggerAggregator.HttpService;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAggregator
{
    public class SwaggerAggregatorProvider : ISwaggerProvider
    {
        private readonly SwaggerHttpClient _client;
        private readonly SwaggerAggregatorOptions _options;

        public SwaggerAggregatorProvider(SwaggerHttpClient client, IOptions<SwaggerAggregatorOptions> options)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public OpenApiDocument GetSwagger(string documentName, string host = null, string basePath = null)
        {
            var services = _options.Services.Where(x => x.Version == documentName).ToList();
            var version = services.FirstOrDefault(x => x.Version == documentName)?.Version;

            var builder = new OpenApiDocumentBuilder();
            builder.SetServer(_options.Servers);
            builder.SetInfo(_options.Info, version);

            foreach (var item in services)
            {
                var currentServiceDocument = _client.GetDocument(item.Url).GetAwaiter().GetResult();

                builder.SetPath(currentServiceDocument.Paths, item.RemoveApiPrefix);

                if (currentServiceDocument.Components != null && currentServiceDocument.Components.Schemas.Any())
                {
                    builder.SetSchema(currentServiceDocument.Components.Schemas);
                }
            }

            var result = builder.Build();

            return result;
        }
    }
}
