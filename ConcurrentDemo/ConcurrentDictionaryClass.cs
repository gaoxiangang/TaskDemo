using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConcurrentDemo
{
    public class ConcurrentDictionaryClass
    {
        const string Item = "Dictionary item";
        const int Iterations = 1000000;
        public static string CurrentItem;

        /******
         * 创建一个普通字段集合 一个新的并发字典集合      
         * 测试一百万数据的读写操作
         * 新的并发字典集合写操作比普通要慢，但是读操作却很快。大量线程安全的读操作建议用新的并发字典集合
         * **/
        public void Begion()
        {
            var concurrentDictionary = new ConcurrentDictionary<int, string>();

            var dictionary = new Dictionary<int, string>();

            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < Iterations; i++)
            {
                lock (dictionary)
                {
                    dictionary[i] = Item;
                }
            }
            sw.Stop();
            //获取总运行时间sw.Elapsed
            Console.WriteLine($" Writing to  dictionary with a lovk :{sw.Elapsed}");
            sw.Restart();
            for (int i = 0; i < Iterations; i++)
            {
                concurrentDictionary[i] = Item;
            }
            sw.Stop();
            Console.WriteLine($" Writing to  concurrentDictionary with a lovk :{sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < Iterations; i++)
            {
                lock (dictionary)
                {
                    CurrentItem = dictionary[i];
                }
            }
            sw.Stop();
            Console.WriteLine($" Reading from dictionary with a lock :{sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < Iterations; i++)
            {
                CurrentItem = concurrentDictionary[i];
            }
            sw.Stop();
            Console.WriteLine($" Read from a concurrentDictionary  :{sw.Elapsed}");
            Console.ReadKey();

        }

    }
}
