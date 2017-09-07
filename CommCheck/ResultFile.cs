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

            using (var outputFile = new StreamWriter(Environment.CurrentDirectory + Path.DirectorySeparatorChar + $"{_fileName}", true))
            {
                outputFile.WriteLine("DataSize[KB],Interval[ms],Threads,ErroCountor,average[ms],max[ms],median[ms],std[ms],ave+3sigma[ms],ave-3sigma[ms]");
            }
        }

        public void Write(ExaminClient examin)
        {
            using (var outputFile = new StreamWriter(Environment.CurrentDirectory + Path.DirectorySeparatorChar + $"{_fileName}", true))
            {   
                var result = examin.ExaminResultTimes;
                outputFile.WriteLine(
                    $"{examin.DataSize/1000},{examin.CommInterval},{examin.ThreadNum},{examin.ErrorCountor}," + 
                    $"{result.Average():f2},{result.Max():f2}," +
                    $"{Mean(result):f2},{CalcStd(result):f2}," +
                    $"{ThreeSigma(result):f2}, {ThreeSigma(result, true):f2}"
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

        private static double ThreeSigma(IReadOnlyCollection<double> result, bool isMinath = false)
        {
            if (isMinath)
                return Math.Max(result.Average() - CalcStd(result) * 3, 0);
            return result.Average() + CalcStd(result) * 3;
        }
    }
}