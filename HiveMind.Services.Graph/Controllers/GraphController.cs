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

        [HttpGet]
        public ActionResult GetAll()
        {
            var nodes = _graphService.GetNodes();
            return Ok(nodes);
        }

        // GET: api/Graph/12/Related
        [HttpGet("{id}/Related")]
        public IEnumerable<RelatedNode> Get([FromRoute] string id)
        {
            var nodes = _graphService.GetRelatedNodes(id);
            return nodes;
        }

        // GET: api/Graph/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void PostTest(Node node)
        {
            _graphService.AddNode(node);
        }

        // POST: api/Graph
        [HttpPost("{type}/{id}")]
        public void Post([FromRoute] string type, [FromRoute] string id, dynamic value)
        {
            var node = new Node() { Type = type, Id = id, Entity = value };
            _graphService.AddNode(node);
        }

        // PUT: api/Graph/5
        [HttpPut("{type}/{id}")]
        public void Put(string type, [FromRoute] string id, [FromBody] object value)
        {
            var node = new Node() { Type = type, Id = id, Entity = value };
            _graphService.UpdateNode(node);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _graphService.Delete(id);
        }
    }
}