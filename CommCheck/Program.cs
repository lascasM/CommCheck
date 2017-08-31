#define USE_BSON
#define PERFOMANCE

using System;


namespace CommCheck
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Console.WriteLine("http+jsonで、エンコード→通信→でコードでどれくらいの時間がかかるのかを測定します");

#if USE_BSON && PERFOMANCE
                var examainBase = ExaminBuilder.Instance()
                    .SetPostTest()
                    .SetBsonUsage()
                    .SetDataSizeArray(new[] {5 * 1000, 1000 * 1000, 10 * 1000 * 1000, 20 * 1000 * 1000, 50 * 1000 * 1000, 100 * 1000 * 1000, 500 * 1000 * 1000})
                    .SetCommIntervalMillSec(new[]{0})
                    .SetExaminNum(3000)
                    .SetThreadNum(new[]{1})
                    .Build();
#elif USE_BSON
                    var examainBase = ExaminBuilder.Instance()
                    .SetPostTest()
                    .SetBsonUsage()
                    .SetDataSizeArray(new[] {1000 * 1000})
                    .SetCommIntervalMillSec(new[]{0, 10, 20})
                    .SetExaminNum(5000)
                    .SetThreadNum(new[]{1,5,8})
                    .Build();
#else
                var examainBase = ExaminBuilder.Instance()
                    .SetPostTest()
                    .SetDataSizeArray(new[] {5 * 1000, 1000 * 1000, 500 * 1000 * 1000})
                    .SetCommIntervalMillSec(new[]{0, 10, 20})
                    .SetExaminNum(20000)
                    .SetThreadNum(new[]{1, 5})
                    .Build();                    
#endif

                examainBase.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}