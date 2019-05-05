using HiveMind.Services.Projects.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace HiveMind.Services.Projects.Services
{
    public class ProjectService
    {
        private IMongoDatabase _database;

        public ProjectService()
        {
            var client = new MongoClient();
            _database = client.GetDatabase("ProjectStore");
        }

        public List<Project> GetAll()
        {
            var projects = new List<Project>();
            var collections = _database.GetCollection<Project>("Project");
            var filter = Builders<Project>.Filter.Empty;
            projects = collections.Find(filter).ToList();
            return projects;
        }
    }
}