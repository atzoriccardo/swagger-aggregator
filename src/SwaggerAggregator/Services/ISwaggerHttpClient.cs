using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerAggregator.HttpService
{
    public interface ISwaggerHttpClient
    {
        Task<OpenApiDocument> GetDocument(string endpoint);
    }
}
