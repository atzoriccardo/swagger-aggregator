using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerAggregator.HttpService
{
    public class SwaggerHttpClient : ISwaggerHttpClient
    {
        private readonly HttpClient _client;

        public SwaggerHttpClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<OpenApiDocument> GetDocument(string endpoint)
        {
            var stream = await _client.GetStreamAsync(endpoint);
            var openApiDocument = new OpenApiStreamReader().Read(stream, out var diagnostic);

            if (diagnostic != null && diagnostic.Errors.Any())
            {
                throw new Exception("");
            }
            
            return openApiDocument;
        }
    }
}
