using System;
using System.Collections.Generic;
using System.Text;

namespace HiveMind.Common.Entities
{
    public class Person : IPerson
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
