using System.Collections.Generic;
using System.Linq;

namespace CommCheck
{
    internal class ExaminBuilder
    {
        private enum TestType
        {
            Post,
            Get
        }

        private TestType _testType;
        private int[] _dataSizeArray;
        private int[] _commIntercalMillSec;
        private int _examinNum;
        private int[] _threadNum;

        public static ExaminBuilder Instance()
        {
            return new ExaminBuilder();
        }

        private ExaminBuilder()
        {
            _testType = TestType.Post;
        }


        public ExaminBuilder SetPostTest()
        {
            _testType = TestType.Post;
            return this;
        }

        public ExaminBuilder SetDataSizeArray(int[] dataSizeArray)
        {
            _dataSizeArray = dataSizeArray;
            return this;
        }

        public ExaminBuilder SetCommIntervalMillSec(int[] commIntercalMillSec)
        {
            _commIntercalMillSec = commIntercalMillSec;
            return this;
        }

        public ExaminBuilder SetExaminNum(int examinNum)
        {
            _examinNum = examinNum;
            return this;
        }

        public ExaminBuilder SetThreadNum(int[] threadNum)
        {
            _threadNum = threadNum;
            return this;
        }

        public ExaminBase Build()
        {
            if (_testType == TestType.Post)
                return new ExaminBase(
                    GeneratePostExaminClient(),
                    new ExaminServer()
                );

            return null;
        }

        private List<ExaminClient> GeneratePostExaminClient()
        {
            var retList = new List<ExaminClient> {};
            retList.AddRange(
                from commInterval in _commIntercalMillSec 
                from dataSize in _dataSizeArray 
                from threadNum in _threadNum
                    select new PostExaminClient(dataSize, commInterval, _examinNum, threadNum)
            );

            return retList;
        }
    }
}