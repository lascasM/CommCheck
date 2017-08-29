using System.Collections.Concurrent;

namespace CommCheck
{
    public abstract class Examin
    {
        protected ConcurrentBag<int> ExaminResultTimes;
        protected readonly int _commInterval;
        protected readonly int _examinNum;

        protected Examin(int commInterval, int examinNum, ConcurrentBag<int> examinResultTimes)
        {
            _commInterval = commInterval;
            _examinNum = examinNum;
            ExaminResultTimes = examinResultTimes;
        }

        public abstract void Execute();
    }
}