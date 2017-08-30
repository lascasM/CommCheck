using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace CommCheck
{
    public abstract class ExaminClient
    {
        public ConcurrentBag<double> ExaminResultTimes { get; }

        private readonly int _dataSize;
        protected readonly int ThreadNum;
        protected readonly int CommInterval;
        protected readonly int ExaminNum;
        public Stopwatch TotalTimer { get; private set; } 
        
        // propertyは、直接スレッドセーフインクリメントが使えないので、トリッキーなことをしている
        private int _errorCouter;

        public int ErrorCountor => _errorCouter;

        protected void IncrementErrorCounter() { Interlocked.Increment(ref _errorCouter); }

        protected ExaminClient(int dataSize, int commInterval, int examinNum, int threadNum)
        {
            _dataSize = dataSize;
            CommInterval = commInterval;
            ExaminNum = examinNum;
            ThreadNum = threadNum;
            TotalTimer = new Stopwatch();
            ExaminResultTimes = new ConcurrentBag<double>();;
        }

        public abstract void Execute();    

        protected byte[] MakeRandomBinary()
        {
            var rnd = new Random();
            var byteLength = rnd.Next((int) (_dataSize * 0.95), (int) (_dataSize * 1.05));
            
            var ret = new byte[byteLength];
            rnd.NextBytes(ret);
                
            return ret;
        }
    }
}