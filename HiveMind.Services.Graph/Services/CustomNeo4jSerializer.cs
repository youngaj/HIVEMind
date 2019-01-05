using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace HiveMind.Services.Graph.Services
{
    public class CustomNeo4jSerializer : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.NullValueHandling = NullValueHandling.Include;
            property.DefaultValueHandling = DefaultValueHandling.Include;
            property.ShouldSerialize = o => true;
            return property;
        }
    }
}