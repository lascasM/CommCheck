using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public ExaminBase Build()
        {
            List<Examin> examinList;

            if (_testType == TestType.Post)
            {
                examinList = GeneratePostExamin();
            }
            
            return new ExaminBase(examinList);
        }

        private List<Examin> GeneratePostExamin()
        {
            var examinResultTimes = new ConcurrentBag<int>;
            var retList = new List<Examin> {};
            retList.AddRange(
                from commInterval in _commIntercalMillSec from dataSize in _dataSizeArray 
                select new PostExamin(dataSize, commInterval, _examinNum, examinResultTimes)
            );

            return retList;
        }
    }
}