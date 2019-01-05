using HiveMind.Common.General;
using HiveMind.Services.Graph.Entities;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
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

            _graphClient.Cypher
                .Merge("(node:" + node.Type + " { Id: {id}, Type: {type} })")
                .OnCreate()
                .Set("node = {nodeEntity}")
                .WithParams(new
                {
                    id = node.Id,
                    type = node.Type,
                    nodeEntity = node
                }).ExecuteWithoutResults();

            return result;
        }

        public Entities.Node GetNode(string type, string id)
        {
            var node = _graphClient.Cypher
                        .Match("(n)")
                        .Where<Entities.Node>(n => n.Type == type && n.Id == id)
                        .Return(n => n.As<Node<Entities.Node>>())
                        .Results
                        .Single();
            return node.Data;
        }

        public List<Entities.Node> GetNodes()
        {
            var entities = new List<Entities.Node>();
            entities = _graphClient.Cypher
                        .Match("(entity)")
                            .Return(entity => entity.As<Entities.Node>())
                            .Results
                            .ToList();
            return entities;
        }
        public List<RelatedNode> GetRelatedNodes(string type, string id)
        {
            var results = _graphClient.Cypher
                            .Match("(a)-[r]-(b)")
                            .Where<Entities.Node>(a => a.Type == type && a.Id == id)
                            .Return((a, r, b) => new
                            {
                                source = a.As<Entities.Node>(),
                                target = b.As<Entities.Node>(),
                                relationship = r.As<Relationship>().RelationshipTypeKey,
                                relationshipData = r.As<Relationship>().Data
                            })
                            .Results;

            var relatedEntities = new List<RelatedNode>();

            foreach (var item in results)
            {
                var relatedEntity = new RelatedNode();
                relatedEntity.Relationship = new Edge() { Data = item.relationshipData, Name = item.relationship };
                relatedEntity.Node = item.target;
                relatedEntities.Add(relatedEntity);
            }

            return relatedEntities;
        }

        public List<RelatedNode> GetRelatedNodes2(string key)
        {
            var query = new CypherFluentQuery(_graphClient)
                            .Match("(findEntity)-[(relation)]->(relatedEntity)")
                            .Return((findEntity, relation, relatedEntity) => new
                            {
                                entity = findEntity.As<Entities.Node>(),
                                r = relation.As<Relationship>(),
                                relatedEntity = relatedEntity.As<Entities.Node>()
                            });

            var queryText = query.Query.QueryText;
            var paramText = query.Query.QueryParameters;
            var results = query.Results.ToList();

            var relatedEntities = new List<RelatedNode>();
            foreach (var item in results)
            {
                var relatedEntity = new RelatedNode();
                relatedEntity.Relationship = new Edge() { Data = item.r.Data, Name = item.r.RelationshipTypeKey };
                relatedEntity.Node = item.relatedEntity;
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