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
 
                #define USE_BSON
                #if USE_BSON
                var examainBase = ExaminBuilder.Instance()
                    .SetPostTest()
                    .SetBsonUsage()
                    .SetDataSizeArray(new[] {5 * 1000, 1000 * 1000, 500 * 1000 * 1000})
                    .SetCommIntervalMillSec(new[]{0, 10, 20})
                    .SetExaminNum(20000)
                    .SetThreadNum(new[]{1, 5})
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