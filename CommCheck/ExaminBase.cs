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
            var ct = tokenSource2.Token;
            Task.Factory.StartNew(()=>
            {
                _examinServer.StartListen(ct);
            }, ct);
            
            Console.WriteLine("==== 測定開始 ====");

            var resultFile = new ResultFile();
            var timerResultFile = new TimerResult();
            for (var i = 0; i < _examinList.Count; i++)
            {
                var examin = _examinList[i];
                
                Console.Write($"測定中[DataSize:{examin.DataSize:n0}, Interval:{examin.CommInterval}, ThreadNum:{examin.ThreadNum}] >>> ");

                examin.Execute();

                resultFile.Write(examin);
                timerResultFile.Write(examin);
                
                Thread.Sleep(2000);
                
                Console.WriteLine($" <<<  測定完了[{i}](測定時間:{examin.TotalTimer.Elapsed.TotalSeconds:n1}[sec])");
            }
            
            Console.WriteLine("==== 測定完了 ====");

            Console.ReadLine();
            tokenSource2.Cancel();
        }
    }
}