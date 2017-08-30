using System;
using System.Collections.Generic;
using System.Net;
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
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());

            TotalTimer.Stop();
        }

        private void ExecuteExmain()
        {
            for (var i = 1; i <= ExaminNum; i++) // 表示のために、1からスタート
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

                    if (i % 500 == 0)
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

        private void PostTo(Uri uri, byte[] bson)
        {
            var webTask = WebPageAsync.Post(uri, bson);

            webTask.Wait();
            var result = webTask.Result; // 結果を取得

            // 取得結果を使った処理
            if (result == null || result.StatusCode != HttpStatusCode.OK)
                throw new ApplicationException();
        }
    }
}