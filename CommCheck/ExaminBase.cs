using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommCheck
{
    internal class ExaminBase
    {
        private readonly List<ExaminClient> _examinList;
        private readonly ExaminServer _examinServer;

        public ExaminBase(List<ExaminClient> examinList, ExaminServer examinServer)
        {
            _examinList = examinList;
            _examinServer = examinServer;
        }

        public void Execute()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;
            Task.Factory.StartNew(()=>
            {
                _examinServer.StartListen(ct);
            }, ct);

            var resultFile = new ResultFile();
            for (var i = 0; i < _examinList.Count; i++)
            {
                Console.Write($"測定中{i} >>>");

                var examin = _examinList[i];
                examin.Execute();

                resultFile.Write(examin);
                
                Console.WriteLine($"<<<  測定完了{i}(測定時間:{examin.TotalTimer.Elapsed.TotalSeconds:n1}[sec])  ====");
            }

            tokenSource2.Cancel();
        }


    }
}