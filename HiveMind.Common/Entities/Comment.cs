using HiveMind.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiveMind.Common.General
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
