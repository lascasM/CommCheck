using System;
using System.Runtime.Serialization;

namespace CommCheck
{
    [DataContract]
    public class Person
    {
        [DataMember(Name="name")]
        public string Name { get; set; }
 
        [DataMember(Name = "Binary")]
        public byte[] Binary { get; set; }
        
        public Person(string name, byte[] binary)
        {
            Name = name;
            Binary = binary;
        }

        public static Person CreatePerson(string name)
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

        public override string ToString()
        {
            return 
$@"Name: {Name} 
Binary: {Convert.ToBase64String(Binary)}
";
        }
    }
}