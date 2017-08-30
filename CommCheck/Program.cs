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
                Console.WriteLine("測定中");
                
                var examainBase = ExaminBuilder.Instance()
                    .SetPostTest()
                    .SetDataSizeArray(new int[] {5 * 1000/*, 50 * 1000, 1000 * 1000, 500 * 1000 * 1000, 2000 * 1000 * 1000*/})
                    .SetCommIntervalMillSec(new int[]{/*10, 15, 20, */30})
                    .SetExaminNum(30000)
                    .SetThreadNum(new int[]{3})
                    .Build();
                
                examainBase.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}