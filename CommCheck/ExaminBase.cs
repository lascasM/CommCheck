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
                
            foreach (var examin in _examinList)
            {
                examin.Execute();
                
                // サンプルとして、取得したHTMLデータの<h1>タグ以降を一定長だけ表示
                Console.WriteLine("====  測定結果(測定時間:{0:n1}[sec])  ====", examin.TotalTimer.Elapsed.TotalSeconds);
                Console.WriteLine("平均[ms]　　　    ：{0:n2}", examin.ExaminResultTimes.Average());
                Console.WriteLine("最大値[ms]　　    ：{0:n2}", examin.ExaminResultTimes.Max());
                Console.WriteLine("標準偏差[ms]　    ：{0:n2}", PopulationStandardDeviation(examin.ExaminResultTimes));
                Console.WriteLine("平均 + 3シグマ[ms]：{0:n2}", examin.ExaminResultTimes.Average() + PopulationStandardDeviation(examin.ExaminResultTimes) * 3);
                Console.WriteLine("====  通信失敗回数 : {0}  ====", examin.ErrorCountor);
            }

            tokenSource2.Cancel();
        }

        private double PopulationStandardDeviation(IReadOnlyCollection<double> pValues)
        {
            //平均を取得
            var lAverage = pValues.Average();
 
            //「σの二乗×データ数」まで計算
            var lStandardDeviation = pValues.Sum(fValue => (fValue - lAverage) * (fValue - lAverage));

            //σを算出して返却
            return Math.Sqrt(lStandardDeviation / pValues.Count);
        }
    }
}