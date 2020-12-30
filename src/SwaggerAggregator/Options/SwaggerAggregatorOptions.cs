using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAggregator
{
    public class SwaggerAggregatorOptions
    {
        public Info Info { get; set; }

        public List<Server> Servers { get; set; }

        public List<Service> Services { get; set; }
    }

    public class Info
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class Server
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
    }

    public class Service
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Version { get; set; }

        public bool RemoveApiPrefix { get; set; }
    }
}
