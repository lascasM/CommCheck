using System;
using System.Collections.Concurrent;

namespace CommCheck
{
    internal class PostExamin : Examin
    {
        private readonly int _dataSize;

        public PostExamin(int dataSize, int commInterval, int examinNum, ConcurrentBag<int> resultTimes) : 
            base(commInterval, examinNum, resultTimes)
        {
            _dataSize = dataSize;
        }

        public override void Execute()
        {
            for (var i = 0; i < _examinNum; i++)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                byte[] data = CreateByteArray(_dataSize);
                
            }
        }
    }
}