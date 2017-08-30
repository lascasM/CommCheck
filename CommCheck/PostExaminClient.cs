using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CommCheck
{
    internal class PostExaminClient : ExaminClient
    {
        public PostExaminClient(int dataSize, int commInterval, int examinNum, int threadNum) :
            base(dataSize, commInterval, examinNum, threadNum)
        {
        }

        public override void Execute()
        {
            TotalTimer.Start();
            
            var tasks = new List<Task>();

            for (var i = 0; i < ThreadNum; i++)
            {
                var t = new Task(ExecuteExmain);
                t.Start();
            }

            Task.WaitAll(tasks.ToArray());
            
            TotalTimer.Stop();
        }

        private void ExecuteExmain()
        {
            for (var i = 0; i < ExaminNum; i++)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                try
                {
                    var person = Person.CreatePerson(i.ToString(), MakeRandomBinary());

                    var bson = BsonSerialiser.SerializeToBson(person);

                    PostTo(
                        new Uri("http://localhost:12345/"),
                        bson
                    );

                    if (i % 1000 == 0)
                        Console.Write($"{i}, ");
                }
                catch
                {
                    IncrementErrorCounter();
                }

                Thread.Sleep(CommInterval);

                sw.Stop();
                ExaminResultTimes.Add(sw.Elapsed.TotalMilliseconds);
            }
        }

        private async void PostTo(Uri uri, byte[] bson)
        {
                var result = await PostImlp(uri, bson);

                if (result == null)
                    throw new FieldAccessException();
        }

        private static async Task<string> PostImlp(Uri uri, byte[] bson)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.PostAsync(
                    uri,
                    new ByteArrayContent(bson)
                );
    
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}