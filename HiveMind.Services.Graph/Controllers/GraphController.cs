using HiveMind.Services.Graph.Entities;
using HiveMind.Services.Graph.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HiveMind.Services.Graph.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private IGraphService _graphService { get; set; }

        public GraphController(IGraphService graphService)
        {
            this._graphService = graphService;
        }

        // GET: api/Graph
        [HttpGet]
        public ActionResult GetAll()
        {
            var nodes = _graphService.GetNodes();
            return Ok(nodes);
        }

        // GET: api/Graph/5
        [HttpGet("{type}/{id}")]
        public ActionResult Get(string type, string id)
        {
            var node = _graphService.GetNode(type, id);
            return Ok(node);
        }

        // GET: api/Graph/12/Related
        [HttpGet("{type}/{id}/Related")]
        public IEnumerable<RelatedNode> GetRelated(string type, string id)
        {
            var nodes = _graphService.GetRelatedNodes(type, id);
            return nodes;
        }

        // POST: api/Graph
        [HttpPost]
        public void PostTest(Node node)
        {
            _graphService.AddNode(node);
        }

        // PUT: api/Graph
        [HttpPut]
        public void Put(Node node)
        {
            _graphService.UpdateNode(node);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _graphService.Delete(id);
        }

        // GET: api/Graph/12/Related
        [HttpPost("{type}/{id}/Related")]
        public ActionResult SetRelated(string type, string id, Node target)
        {
            var source = new Node() { Id = id, Type = type };
            var edge = new Edge() { Name = "RELATED_TO" };
            _graphService.AddEdge(source, target, edge);
            return Ok();
        }

    }
}