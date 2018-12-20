using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiveMind.Common.Services
{
    public static class SerializationService
    {
        public static string Serialize(object entity)
        {
            var json = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return json;
        }

        public static string SerializeExcludeNulls(object entity)
        {
            var json = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });
            return json;
        }

        public static object DeSerialize(string val)
        {
            var obj = JsonConvert.DeserializeObject(val);
            return obj;
        }
    }
}
