using HiveMind.Common.General;
using HiveMind.Common.Services;
using HiveMind.Services.Graph.Entities;
using Microsoft.Extensions.Configuration;
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

        IResult AddEdge(string source, string target, Edge relationship);

        void Delete(string id);

        List<Entities.Node> GetNodes();

        List<RelatedNode> GetRelatedNodes(string key);

        void UpdateNode(Entities.Node node);
    }

    public class GraphService : IGraphService
    {
        private readonly IGraphClient _graphClient;

        public GraphService(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public IResult AddEdge(string source, string target, Edge relationship)
        {
            var result = ResultFactory.CreateInstance();
            result.Type = ResultType.Successful;

            var relationshipName = relationship.Name;
            _graphClient.Cypher
                .Match("(sourceNode)", "(targetNode)")
                .Where((Entities.Node sourceNode) => sourceNode.Id == source)
                .AndWhere((Entities.Node targetNode) => targetNode.Id == target)
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
            //    .Return(entity => entity.As<Node<Entities.Node>>())
            //    .Results
            //    .Single();
            //if (newNode == null)
            //{
            //    result.Type = ResultType.Failure;
            //    result.AddMessage("New node not found after insert");
            //}

            return result;
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

        public List<RelatedNode> GetRelatedNodes(string key)
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
            var updatedNode = _graphClient.Cypher
                .Merge("(entity:" + node.Type + " { Id: {id} })")
                .OnMatch()
                .Set("entity = {nodeEntity}")
                .WithParams(new
                {
                    Id = node.Id,
                    nodeEntity = node
                })
                .Return(entity => entity.As<Node<Entities.Node>>())
                .Results
                .Single();

            if (updatedNode == null)
                throw new Exception("Node update failed.");
        }
    }
}