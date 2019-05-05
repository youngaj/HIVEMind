using System;
using System.Collections.Generic;

namespace HiveMind.Services.Projects.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Acronym { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }

        public List<ProjectMember> Personnel { get; set; }
    }
}