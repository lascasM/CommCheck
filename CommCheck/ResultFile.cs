using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommCheck
{
    internal class ResultFile
    {
        private readonly string _fileName;

        public ResultFile()
        {
            var dt = DateTime.Now;
            _fileName = "result_" + dt.Year + dt.Month + dt.Day + dt.Hour + dt.Minute + ".csv";

            using (var outputFile = new StreamWriter(Environment.CurrentDirectory + $"\\{_fileName}", true))
            {
                outputFile.WriteLine("DataSize,Interval,Threads,average,max,median,std,3sigma");
            }
        }

        public void Write(ExaminClient examin)
        {
            using (var outputFile = new StreamWriter(Environment.CurrentDirectory + $"\\{_fileName}", true))
            {
                
                var result = examin.ExaminResultTimes;
                outputFile.WriteLine(
                    $"{examin.DataSize},{examin.CommInterval},{examin.ThreadNum}," + 
                    $"{result.Average():n2},{result.Max():n2},{Mean(result):n2},{CalcStd(result):n2},{(result.Average() + CalcStd(result) * 3):n2}"
                );
            }
        }

        private static double Mean(ConcurrentBag<double> xs)
        {
            var ys = xs.OrderBy(x => x).ToList();
            return (xs.ElementAt(ys.Count/2) + ys.ElementAt((ys.Count-1)/2)) / 2;
        }

        private static double CalcStd(IReadOnlyCollection<double> pValues)
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