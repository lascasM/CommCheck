using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CommCheck
{
    internal class PostExaminClient : ExaminClient
    {
        public PostExaminClient(bool useBson, int dataSize, int commInterval, int examinNum, int threadNum) :
            base(useBson, dataSize, commInterval, examinNum, threadNum)
        {}

        public override void Execute()
        {
            TotalTimer.Start();

            var tasks = new List<Task>();

            for (var i = 0; i < ThreadNum; i++)
            {
                var displayed = i == 0;
                var t = new Task(()=>
                {
                    ExecuteExmain(displayed);
                });
                t.Start();
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());

            TotalTimer.Stop();
        }

        private void ExecuteExmain(bool displayed)
        {
            for (var i = 1; i <= ExaminNum; i++) // 表示のために、1からスタート
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                try
                {
                    var binaryData = CreateSendBinary(i);

                    PostTo(
                        new Uri("http://localhost:12345/"),
                        binaryData
                    );
                }
                catch
                {
                    IncrementErrorCounter();
                }

                Thread.Sleep(CommInterval);

                sw.Stop();
                ExaminResultTimes.Add(sw.Elapsed.TotalMilliseconds);

                if (i % 500 == 0 && displayed)
                    Console.Write($"{i}, ");
            }
        }

        private byte[] CreateSendBinary(int i)
        {
            if (!_useBson) 
                return MakeRandomBinary();
            
            var person = Person.CreatePerson(i.ToString(), MakeRandomBinary());
            return BsonSerialiser.SerializeToBson(person);
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