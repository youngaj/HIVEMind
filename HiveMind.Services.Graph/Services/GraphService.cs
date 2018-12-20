using HiveMind.Common.General;
using HiveMind.Services.Graph.Entities;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver.V1;
using System.Collections.Generic;
using System.Linq;

namespace HiveMind.Services.Graph.Services
{
    public interface IGraphService
    {
        IResult AddNode(Node node);

        IResult AddEdge(string source, string target, Edge relationship);

        List<Node> GetNodes();

        List<RelatedNode> GetRelatedNodes(string key);
    }

    public class GraphService : IGraphService
    {
        private readonly IDriver _driver;

        public GraphService(IConfiguration configuration)
        {
            var uri = configuration.GetValue<string>("Neo4j:uri");
            _driver = GraphDatabase.Driver(uri);
        }

        public IResult AddEdge(string source, string target, Edge relationship)
        {
            throw new System.NotImplementedException();
        }

        public IResult AddNode(Node node)
        {
            throw new System.NotImplementedException();
        }

        public List<Node> GetNodes()
        {
            var nodes = new List<Node>();
            var session = _driver.Session();
            IStatementResult result = session.Run("Match (a) Return a");
            nodes = result.Select(x => x.As<Node>()).ToList();
            return nodes;
        }

        public List<RelatedNode> GetRelatedNodes(string key)
        {
            var nodes = new List<RelatedNode>();
            var session = _driver.Session();
            IStatementResult result = session.Run($"Match (a)-[r]-(b) Where a.Id ='{key}' Return r, b");
            nodes = result.Select(x => new RelatedNode()
            {
                Node = x["b"].As<Node>(),
                Relationship = x["r"].As<Edge>()
            }).ToList();
            return nodes;
        }
    }
}