using System;
using System.Net;
using System.Text;

namespace CommCheck
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                var listener = CreatHttpListener();
                var examainBase = ExaminBuilder.Instance()
                    .SetPostTest()
                    .SetDataSizeArray(new int[] {5 * 1000/*, 50 * 1000, 1000 * 1000, 500 * 1000 * 1000, 2000 * 1000 * 1000*/})
                    .SetCommIntervalMillSec(new int[]{/*10, 15, 20, */30})
                    .SetExaminNum(30000)
                    .Build();

                var testNumber = 1;
                while (true)
                {
                    var context = listener.GetContext();

                    if (context.Request.HttpMethod == "POST")
                    {
                        ReactPostRequest(context);
                    }
                    else
                    {
                        ResponseForGetRequest(context, testNumber);
                    }
                    
                    testNumber++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static HttpListener CreatHttpListener()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:12346/");
            listener.Start();
            return listener;
        }

        private static void ReactPostRequest(HttpListenerContext context)
        {
            var res = context.Response;
            res.StatusCode = 200;
            res.Close();
        }

        private static void ResponseForGetRequest(HttpListenerContext context, int testNumber)
        {
            var res = context.Response;
            res.StatusCode = 200;

            var p1 = Person.CreatePerson(testNumber.ToString());
            Console.WriteLine(p1.ToString());

            var bson = BsonSerialiser.SerializeToBson(p1);

            var content = bson;
            if (IsTest(context.Request.RawUrl))
            {
                var person = BsonSerialiser.DeserializeBsonFrom<Person>(bson);
                content = Encoding.UTF8.GetBytes(person.ToString());
            }

            res.OutputStream.Write(content, 0, content.Length);
            res.Close();
        }

        private static bool IsTest(string requestRawUrl)
        {
            return requestRawUrl.Contains("test");
        }
    }
}