using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace CommCheck
{
    internal class BsonSerialiser
    {
        public static byte[] SerializeToBson<TType>(TType person)
        {
            var ms = new MemoryStream();
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, person);
            }

            return ms.ToArray();
        }

        public static TType DeserializeBsonFrom<TType>(byte[] bson)
        {         
            var ms = new MemoryStream(bson);
            using (var reader = new BsonReader(ms))
            {
                var serializer = new JsonSerializer();
            
                return serializer.Deserialize<TType>(reader);
            }
        }
    }
}