using System;
using System.IO;
using System.Linq;

namespace CommCheck
{
    internal class TimerResult
    {
        private readonly string _fileName;

        public TimerResult()
        {
            var dt = DateTime.Now;
            _fileName = "timerResult_" + dt.Year + dt.Month + dt.Day + dt.Hour + dt.Minute + ".csv";

            using (var outputFile = new StreamWriter(Environment.CurrentDirectory + $"\\{_fileName}", true))
            {
                outputFile.WriteLine("DataSize,Interval,Threads,time");
            }
        }

        public void Write(ExaminClient examin)
        {
            using (var outputFile = new StreamWriter(Environment.CurrentDirectory + $"\\{_fileName}", true))
            {

                foreach (var result in examin.ExaminResultTimes)
                {
                    outputFile.WriteLine(
                        $"{examin.DataSize},{examin.CommInterval},{examin.ThreadNum},{result:n2}"
                    );
                }
            }
        }
    }
}