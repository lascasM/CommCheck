﻿//#define USE_BSON
//#define PERFOMANCE

using System;

namespace CommCheck
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                Console.WriteLine("http+jsonで、エンコード→通信→でコードでどれくらいの時間がかかるのかを測定します");

#if PERFOMANCE
                var examinBuilder = ExaminBuilder.Instance()
                    .SetPostTest()
                    .SetDataSizeArray(new[]
                    {
                        500 * 1000 * 1000
                    })
                    .SetCommIntervalMillSec(new[] {0})
                    .SetExaminNum(1000)
                    .SetThreadNum(new[] {1});
#else
                var examinBuilder = ExaminBuilder.Instance()
                    .SetPostTest()
                    .SetDataSizeArray(new[] {10 * 1000, 100 * 1000, 1000 * 1000})
                    .SetCommIntervalMillSec(new[]{0, 10, 20})
                    .SetExaminNum(5000)
                    .SetThreadNum(new[]{1,5,8});
#endif
#if USE_BSON
                examinBuilder = examinBuilder.SetBsonUsage();       
#endif
                var examainBase = examinBuilder.Build();
                examainBase.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}