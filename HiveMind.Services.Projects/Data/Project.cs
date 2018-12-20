using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB;

namespace HiveMind.Services.Projects.Data
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Acronym { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }
    }
}
