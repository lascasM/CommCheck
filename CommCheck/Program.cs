using System;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace CommCheck
{
 
    [DataContract]
    public class Person
    {
        public Person(string name, byte[] binary)
        {
            Name = name;
            Binary = binary;
        }

        [DataMember(Name="name")]
        public string Name { get; set; }
 
        [DataMember(Name = "Binary")]
        public byte[] Binary { get; set; }

        public override string ToString()
        {
            return 
$@"Name: {Name} 
Binary: {Convert.ToBase64String(Binary)}
";
        }
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {   
            try
            {
                var listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:12346/");
                listener.Start();

                var testNumber = 1;
                while (true)
                {
                    var context = listener.GetContext();
                    var res = context.Response;
                    res.StatusCode = 200;

                    var p1 = CreatePerson(testNumber.ToString());
                    Console.WriteLine(p1.ToString());
                    
                    var bson = SerializeToBson(p1);
                    
                    var person = DeserializeBsonFrom(bson);
                    
                    var content = Encoding.UTF8.GetBytes(person.ToString());
                    res.OutputStream.Write(content, 0, content.Length);
                    res.Close();
                    testNumber++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static Person CreatePerson(string name)
        {
            return new Person(name, MakeRandomBinary());
        }

        private static byte[] MakeRandomBinary()
        {
            var rnd = new Random();
            var byteLength = rnd.Next(54 * 1000, 56 * 1000);
            
            var ret = new byte[byteLength];
            rnd.NextBytes(ret);
                
            return ret;
        }

        private static byte[] SerializeToBson(Person person)
        {
            var ms = new MemoryStream();
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, person);
            }

            return ms.ToArray();
        }

        private static Person DeserializeBsonFrom(byte[] bson)
        {         
            var ms = new MemoryStream(bson);
            using (var reader = new BsonReader(ms))
            {
               var serializer = new JsonSerializer();
            
               return serializer.Deserialize<Person>(reader);
            }
        }
    }
}