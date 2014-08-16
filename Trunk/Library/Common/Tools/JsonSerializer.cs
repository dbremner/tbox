using System;
using System.IO;
using Newtonsoft.Json;

namespace Mnk.Library.Common.Tools
{
    public static class JsonSerializer
    {
        public static T DeserializeFromStream<T>(Stream stream)
        {
            using (var r = new StreamReader(stream))
            {
                using (var jr = new JsonTextReader(r))
                {
                    return new Newtonsoft.Json.JsonSerializer().Deserialize<T>(jr);
                }
            }
        }

        public static T DeserializeFromString<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        public static object DeserializeFromStream(Type type, Stream stream)
        {
            using (var r = new StreamReader(stream))
            {
                using (var jr = new JsonTextReader(r))
                {
                    return new Newtonsoft.Json.JsonSerializer().Deserialize(jr, type);
                }
            }
        }

        public static void SerializeToStream<T>(T data, Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                using (var textWriter = new JsonTextWriter(writer))
                {
                    new Newtonsoft.Json.JsonSerializer().Serialize(textWriter, data);
                }
            }
        }

        public static void SerializeToStream(object data, Type type, Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                using (var textWriter = new JsonTextWriter(writer))
                {
                    new Newtonsoft.Json.JsonSerializer().Serialize(textWriter, data, type);
                }
            }
        }

        public static string SerializeToString<T>(T target)
        {
            return JsonConvert.SerializeObject(target);
        }

    }
}
