using HiveMind.Common.Entities;
using System;

namespace HiveMind.Services.Projects.Entities
{
    public class Person : IPerson
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}