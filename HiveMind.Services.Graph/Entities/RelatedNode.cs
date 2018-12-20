using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiveMind.Services.Graph.Entities
{
    public class RelatedNode
    {
        public Node Node { get; set; }
        public Edge Relationship { get; set; }
    }
}
