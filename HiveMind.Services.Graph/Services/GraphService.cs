using HiveMind.Common.General;
using HiveMind.Services.Graph.Entities;
using Neo4jClient;
using Neo4jClient.Cypher;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HiveMind.Services.Graph.Services
{
    public interface IGraphService
    {
        IResult AddNode(Entities.Node node);

        IResult AddEdge(Entities.Node source, Entities.Node target, Edge relationship);

        void Delete(string id);

        List<Entities.Node> GetNodes();

        List<RelatedNode> GetRelatedNodes(string type, string id);

        void UpdateNode(Entities.Node node);

        Entities.Node GetNode(string type, string id);
    }

    public class GraphService : IGraphService
    {
        private readonly IGraphClient _graphClient;

        public GraphService(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public IResult AddEdge(Entities.Node source, Entities.Node target, Edge relationship)
        {
            var result = ResultFactory.CreateInstance();
            result.Type = ResultType.Successful;

            var relationshipName = relationship.Name;
            _graphClient.Cypher
                .Match("(sourceNode)", "(targetNode)")
                .Where((Entities.Node sourceNode) => sourceNode.Type == source.Type && sourceNode.Id == source.Id)
                .AndWhere((Entities.Node targetNode) => targetNode.Type == target.Type && target.Id == target.Id)
                .Create("(sourceNode)-[:" + relationshipName + "]->(targetNode)")
                .ExecuteWithoutResults();

            return result;
        }

        public IResult AddNode(Entities.Node node)
        {
            var result = ResultFactory.CreateInstance();
            result.Type = ResultType.Successful;
            var json = JsonConvert.SerializeObject(node.Entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            _graphClient.Cypher
                .Merge("(node:" + node.Type + " { Id: {id}, Type: {type} })")
                .OnCreate()
                .Set("node = {nodeEntity}")
                .WithParams(new
                {
                    id = node.Id,
                    type = node.Type,
                    nodeEntity = json
                }).ExecuteWithoutResults();

            return result;
        }

        public Entities.Node GetNode(string type, string id)
        {
            var serializedNode = _graphClient.Cypher
                        .Match("(n)")
                        .Where<Entities.Node>(n => n.Type == type && n.Id == id)
                        .Return(n => n.As<Node<SerializedNode>>())
                        .Results
                        .Single().Data;
            var node = DeserializeNode(serializedNode);
            return node;
        }

        public List<Entities.Node> GetNodes()
        {
            var entities = _graphClient.Cypher
                        .Match("(entity)")
                            .Return(entity => entity.As<SerializedNode>())
                            .Results
                            .ToList();
            var nodes = entities.Select(x => DeserializeNode(x)).ToList();
            return nodes;
        }

        private Entities.Node DeserializeNode(SerializedNode serializedNode)
        {
            Entities.Node node = null;
            if (serializedNode != null)
            {
                node = new Entities.Node()
                {
                    Id = serializedNode.Id,
                    Type = serializedNode.Type,
                    Entity = JsonConvert.DeserializeObject(serializedNode.Entity),
                };
            }
            return node;
        }

        public List<RelatedNode> GetRelatedNodes(string type, string id)
        {
            var relatedEntities = new List<RelatedNode>();

            var query = new CypherFluentQuery(_graphClient)
                            .Match("(a)-[r]-(b)")
                            .Where<Entities.Node>(a => a.Type == type && a.Id == id)
                            .Return((a, r, b) => new
                            {
                                source = a.As<SerializedNode>(),
                                target = b.As<SerializedNode>(),
                                relationship = r.As<Relationship>().RelationshipTypeKey,
                            });
            var results = query.Results;

            foreach (var item in results)
            {
                var relatedEntity = new RelatedNode();
                relatedEntity.Relationship = new Edge() { Name = "RELATED_TO" };
                relatedEntity.Node = DeserializeNode(item.target);
                relatedEntities.Add(relatedEntity);
            }

            return relatedEntities;
        }

        public void Delete(string id)
        {
            _graphClient.Cypher
                .Match("(nodeEntity { Id: '{id}' })")
                .OptionalMatch("(nodeEntity)-[r]-()")
                .WithParam("id", id)
                .Delete("nodeEntity, r")
                .ExecuteWithoutResults();
        }

        public void UpdateNode(Entities.Node node)
        {
            _graphClient.Cypher
                .Merge("(node:" + node.Type + " { Id: {id}, Type: {type} })")
                .OnMatch()
                .Set("node = {nodeEntity}")
                .WithParams(new
                {
                    id = node.Id,
                    type = node.Type,
                    nodeEntity = node
                }).ExecuteWithoutResults();
        }
    }
}