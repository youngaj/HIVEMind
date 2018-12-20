using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiveMind.Services.Graph.Entities
{
    public class Node
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public object Entity { get; set; }
    }
}
