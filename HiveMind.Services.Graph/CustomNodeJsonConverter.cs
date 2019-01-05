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
            JObject jObject = JObject.Load(reader);
            var entityToken = jObject.Property("Entity").Value;
            //Remove it so it's not deserialized by Json.NET
            jObject.Remove("Entity");

            //Get the dictionary ourselves and deserialize
            var entity = JsonConvert.DeserializeObject(entityToken.ToString());

            //The output
            var output = new Node();
            //Deserialize all the normal properties
            output = JsonConvert.DeserializeObject<Node>(jObject.ToString());
            //serializer.Populate(jObject.CreateReader(), output);

            //Add our entity
            output.Entity = entity;

            //return
            return output;
        }

        public override bool CanConvert(Type objectType)
        {
            //Only can convert if it's of the right type
            return objectType == typeof(Node);
        }
    }
}