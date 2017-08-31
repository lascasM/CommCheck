using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace CommCheck
{
    public abstract class ExaminClient
    {
        public ConcurrentBag<double> ExaminResultTimes { get; }

        public int DataSize { get; }
        public int ThreadNum { get; }
        public int CommInterval { get; }
        protected readonly bool _useBson;
        protected readonly int ExaminNum;
        public Stopwatch TotalTimer { get; } 
        
        // propertyは、直接スレッドセーフインクリメントが使えないので、トリッキーなことをしている
        private int _errorCouter;

        public int ErrorCountor => _errorCouter;

        protected void IncrementErrorCounter() { Interlocked.Increment(ref _errorCouter); }

        protected ExaminClient(bool useBson, int dataSize, int commInterval, int examinNum, int threadNum)
        {
            DataSize = dataSize;
            CommInterval = commInterval;
            _useBson = useBson;
            ExaminNum = examinNum;
            ThreadNum = threadNum;
            TotalTimer = new Stopwatch();
            ExaminResultTimes = new ConcurrentBag<double>();
        }

        public abstract void Execute();    

        protected byte[] MakeRandomBinary()
        {
            var rnd = new Random();
            var byteLength = rnd.Next((int) (DataSize * 0.95), (int) (DataSize * 1.05));

            var ret = Enumerable.Repeat<byte>(30, byteLength).ToArray();
                
            return ret;
        }
    }
}