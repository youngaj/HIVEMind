using System;

namespace HiveMind.Common.Entities
{
    public interface IPerson
    {
        Guid Id { get; }
        string Name { get; }
    }
}