using System;
using HiveMind.Services.Graph.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HiveMind.Services.Graph
{
    public class CustomNodeJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var twd = value as Node;
            if (twd == null)
                return;

            JToken t = JToken.FromObject(value);
            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                var o = (JObject)t;
                var entity = o.Property("Entity");
                o.Remove("Entity");
                var json = JsonConvert.SerializeObject(entity.Value, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                o.Add("Entity", json);
                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType != typeof(Node))
                return null;

            //Load our object
            var output = new Node();
            object entity = null;

            JObject jObject = JObject.Load(reader);
            var entityToken = jObject.Property("Entity")?.Value;
            if(entityToken != null)
            {
                jObject.Remove("Entity");
                entity = JsonConvert.DeserializeObject(entityToken?.ToString());
            }

            //Get the entity ourselves and deserialize
            output = JsonConvert.DeserializeObject<Node>(jObject.ToString());

            //Add our entity
            output.Entity = entity;
            return output;
        }

        public override bool CanConvert(Type objectType)
        {
            //Only can convert if it's of the right type
            return objectType == typeof(Node);
        }
    }
}