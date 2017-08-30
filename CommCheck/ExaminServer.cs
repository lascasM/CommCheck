using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace CommCheck
{
    public class ExaminServer
    {
        private readonly HttpListener _listener;
        
        public ExaminServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:12345/");
        }

        public void StartListen(CancellationToken ct)
        {
            try
            {
                _listener.Start();
                
                var testNumber = 1;
                while (ct.IsCancellationRequested)
                {
                    var context = _listener.GetContext();

                    if (context.Request.HttpMethod == "POST")
                        ReactPostRequest(context);
                    else
                        ResponseForGetRequest(context, testNumber);

                    testNumber++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            void ReactPostRequest(HttpListenerContext context)
            {
                var ms = new MemoryStream();
                context.Request.InputStream.CopyTo(ms);
                BsonSerialiser.DeserializeBsonFrom<Person>(ms.ToArray());
                
                var res = context.Response;
                res.StatusCode = 200;
                res.Close();
            }

            void ResponseForGetRequest(HttpListenerContext context, int testNumber)
            {
                var res = context.Response;
                res.StatusCode = 200;

                var p1 = Person.CreatePerson(testNumber.ToString());

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

            bool IsTest(string requestRawUrl)
            {
                return requestRawUrl.Contains("test");
            }
        }
    }
}